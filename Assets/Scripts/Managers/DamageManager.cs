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
    public Debuff debuff;
    public float amount = 0;
    public bool didCrit = false;
    public bool didKill = false;
    public bool isDoT = false;
    public bool doIFrames = true;
    public bool doDamageNumbers = true;
    public bool doSoundEffect = true;
}

public class DamageManager : MonoBehaviour
{
    [SerializeField] GameObject m_DamageNumberPrefab;

    public static DamageManager m_Instance;
    public class DataEvent : UnityEvent<DamageInstanceData> { }

    public static DataEvent m_DamageInstanceEvent = new DataEvent();

    private void Awake()
    {
        m_Instance = this;
    }

    public DamageOutput DamageInstance(DamageInstanceData data, Vector2 pos)
    {
        if (data.doSoundEffect)
            AudioManager.m_Instance.PlaySound(0);

        Actor actorComponent = data.target.GetComponent<Actor>();
        DamageOutput damageOutput = 0;
        if (data.doIFrames)
        {
            // Damage actor
            damageOutput = actorComponent.TakeDamage(data.amount);
        }
        else
        {
            actorComponent.TakeDamageNoIFrames(data.amount);
        }
        if (damageOutput >= DamageOutput.invalidHit && data.doDamageNumbers)
        {
            // Spawn damage numbers
            GameObject damageNumber = Instantiate(m_DamageNumberPrefab);
            damageNumber.transform.position = pos;
            damageNumber.GetComponent<FloatingDamage>().m_Colour = GetDamageNumberColor(data.damageType);
            damageNumber.GetComponent<FloatingDamage>().m_Damage = data.amount;
            data.didKill = damageOutput == DamageOutput.wasKilled;
            m_DamageInstanceEvent.Invoke(data);
        }
        return damageOutput;
    }

    private Color GetDamageNumberColor(DamageType damageType)
    {
        switch(damageType)
        {
            case DamageType.Fire: return new Color(1, 0.4f, 0);
            case DamageType.Frost: return Color.cyan;
            case DamageType.Light: return Color.white;
            case DamageType.Physical: return Color.red;
            case DamageType.Poison: return Color.green;
            case DamageType.Lightning: return Color.blue;
            default:
                return Color.white;
        }
    }
}
