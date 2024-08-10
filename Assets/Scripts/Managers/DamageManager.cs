using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DamageType
{
    Fire,
    Frost,
    Lightning,
    Poison,
    Light,
    Dark,
    Physical,
    None
}

// An instance of damage dealt to an actor
public class DamageInstanceData
{
    public DamageInstanceData(GameObject _user, GameObject _target) { user = _user; target = _target; }

    public DamageType damageType = DamageType.None;
    public GameObject user;
    public GameObject target;
    public Ability abilitySource;
    public Skill skillSource;
    public float amount = 0;
    public bool didCrit = false;
    public bool didKill = false;
    public bool isDoT = false;
    public bool doDamageNumbers = true;
    public bool doSoundEffect = true;
}

public class DamageManager : MonoBehaviour
{
    [SerializeField] GameObject m_DamageNumberPrefab;

    public static DamageManager m_Instance;
    public class DamageDataEvent : UnityEvent<DamageInstanceData> { }

    public static DamageDataEvent m_DamageInstanceEvent = new DamageDataEvent();

    private readonly float kDamageNumberRandomRadius = 0.25f;

    private void Awake()
    {
        m_Instance = this;
    }

    // Whenever damage is dealt, is goes via the damage manager with all the relevant data passed into it via 'data'.
    // This data then determines what happens such as the colour of the damage numbers, sound effect played etc.
    // This information is then passed to a damage instance event which some skills listen for to check if their conditions are met.
    public Actor.DamageOutput DamageInstance(DamageInstanceData data, Vector2 pos)
    {
        Actor actorComponent = data.target.GetComponent<Actor>();

        float damageDealt = data.amount * (1f - actorComponent.m_DamageResistance);

        if (damageDealt <= 0f) { damageDealt = 1f; }

        Actor.DamageOutput damageOutput = actorComponent.TakeDamage(damageDealt);

        if (damageOutput == Actor.DamageOutput.invalidHit) return damageOutput;

        if (data.doSoundEffect)
        {
            // Play damage sound effect
            if (data.target.CompareTag("Player")) AudioManager.m_Instance.PlaySound(21);
            else AudioManager.m_Instance.PlaySound(0);
        }
        if (data.doDamageNumbers)
        {
            // Spawn damage numbers
            GameObject damageNumber = Instantiate(m_DamageNumberPrefab);
            damageNumber.transform.position = pos + new Vector2(Random.Range(-kDamageNumberRandomRadius, kDamageNumberRandomRadius), Random.Range(-kDamageNumberRandomRadius, kDamageNumberRandomRadius));
            damageNumber.GetComponent<FloatingDamage>().m_Colour = GetDamageNumberColor(data.damageType);
            damageNumber.GetComponent<FloatingDamage>().m_Damage = damageDealt;
            data.didKill = damageOutput == Actor.DamageOutput.wasKilled;
            m_DamageInstanceEvent.Invoke(data);
        }
        IncrementIceDamageDealt(data);
        return damageOutput;
    }

    private Color GetDamageNumberColor(DamageType damageType)
    {
        switch(damageType)
        {
            case DamageType.Fire: return new Color(1, 0.4f, 0);
            case DamageType.Frost: return Color.cyan;
            case DamageType.Light: return Color.white;
            case DamageType.Dark: return Color.magenta;
            case DamageType.Physical: return Color.red;
            case DamageType.Poison: return Color.green;
            case DamageType.Lightning: return Color.blue;
            default:
                return Color.white;
        }
    }

    private void IncrementIceDamageDealt(DamageInstanceData data)
    {
        if (data.damageType != DamageType.Frost) return;
        if (data.target == Player.m_Instance.gameObject) return;

        UnlockManager.m_TrackedStats.iceDamageDealt += data.amount;
    }
}
