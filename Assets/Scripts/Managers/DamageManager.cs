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
    None,
    Healing
}

// An instance of damage dealt to an actor
public class DamageInstanceData
{
    public DamageInstanceData(GameObject _user, GameObject _target) { user = _user; target = _target; }

    public DamageType damageType = DamageType.None;
    public GameObject user;
    public GameObject target;
    public Vector2 hitPosition;
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

    public static DamageDataEvent m_DamageInstanceEvent = new();

    private readonly float kDamageNumberRandomRadius = 0.25f;

    public float m_CritDamageModifier = 1.5f;

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
        bool isSelfDamage = data.target == data.user;

        data.didCrit = data.abilitySource && data.abilitySource.GetTotalStats().critChance > 0f && Random.Range(0f, 1f) < data.abilitySource.GetTotalStats().critChance;
        float critModifier = data.didCrit ? m_CritDamageModifier : 1f;

        float damageDealt = data.amount * critModifier * (1f - actorComponent.m_DamageResistance);

        if (data.target.CompareTag("Player") && Skill.necroBleedEnabled && !data.isDoT) damageDealt *= 0.5f;

        if (data.target.CompareTag("Player") && !data.isDoT) damageDealt = Mathf.Clamp(damageDealt - data.target.GetComponent<Player>().GetStats().armor, 1f, float.MaxValue);

        if (damageDealt <= 0f) { damageDealt = 1f; }

        Actor.DamageOutput damageOutput;

        if (data.isDoT && data.target.CompareTag("Player"))
        {
            damageOutput = data.target.GetComponent<Player>().TakeDOTDamage(damageDealt);
        }
        else
        {
            damageOutput = actorComponent.TakeDamage(damageDealt);
        }

        if (damageOutput == Actor.DamageOutput.invalidHit) return damageOutput;

        if (data.doSoundEffect)
        {
            // Play damage sound effect
            if (data.target.CompareTag("Player")) AudioManager.m_Instance.PlaySound(21);
            else
            {
                if (data.didCrit) AudioManager.m_Instance.PlaySound(33, 0.5f);
                else AudioManager.m_Instance.PlaySound(0);
            }
        }
        else
        {
            if (data.didCrit && !data.target.CompareTag("Player")) AudioManager.m_Instance.PlaySound(33, 0.75f);
        }

        if (data.doDamageNumbers)
        {
            SpawnDamageNumbers(damageDealt, pos, data.damageType, data.didCrit);
            data.didKill = damageOutput == Actor.DamageOutput.wasKilled;
            if (!isSelfDamage) m_DamageInstanceEvent.Invoke(data);
        }

        if (!isSelfDamage) TryIncrementTrackedStats(data);
        return damageOutput;
    }

    public void SpawnDamageNumbers(float amount, Vector2 pos, DamageType damageType, bool isCrit)
    {
        // Spawn damage numbers
        GameObject damageNumber = Instantiate(m_DamageNumberPrefab);
        damageNumber.transform.position = pos + new Vector2(Random.Range(-kDamageNumberRandomRadius, kDamageNumberRandomRadius), Random.Range(-kDamageNumberRandomRadius, kDamageNumberRandomRadius));
        damageNumber.GetComponent<FloatingDamage>().m_Damage = amount;
        if (isCrit) {
            damageNumber.GetComponent<FloatingDamage>().m_Colour = new Color(1, 0.15f, 0);//Color.Lerp(GetDamageNumberColor(damageType), Color.yellow, 0.5f);
            damageNumber.GetComponent<FloatingDamage>().m_StartSize *= 2f;
            damageNumber.GetComponent<FloatingDamage>().m_ShrinkTime *= 1.5f;
            PlayerManager.m_Instance.StartShake(0.1f, 0.1f);
        }
        else
        {
            damageNumber.GetComponent<FloatingDamage>().m_Colour = GetDamageNumberColor(damageType);
        }
    }

    public static Color GetDamageNumberColor(DamageType damageType)
    {
        return damageType switch
        {
            DamageType.Fire => new Color(1, 0.35f, 0),
            DamageType.Frost => Color.cyan,
            DamageType.Light => new Color(1f, 1f, 0.5f),
            DamageType.Dark => Color.magenta,
            DamageType.Physical => Color.red,
            DamageType.Poison => new Color(0, 0.2f, 0),
            DamageType.Lightning => Color.blue,
            DamageType.Healing => Color.green,
            _ => Color.white,
        };
    }

    void TryIncrementTrackedStats(DamageInstanceData data)
    {
        if (data.target == Player.m_Instance.gameObject) return;

        IncrementIceDamageDealt(data);
        IncrementKills(data);
        IncrementDamage(data);
        //IncrementDOTDamage(data);
        IncrementSummonDamage(data);
        IncrementBasicSpellDamage(data);
    }

    private void IncrementIceDamageDealt(DamageInstanceData data)
    {
        if (data.damageType != DamageType.Frost) return;

        UnlockManager.GetTrackedStatWithName("iceDamageDealt").stat += data.amount;
        if (!SteamworksManager.failed) Steamworks.SteamUserStats.AddStat("frost_dmg", data.amount);
    }

    void IncrementKills(DamageInstanceData data)
    {
        if (data.didKill)
        {
            UnlockManager.GetTrackedStatWithName("kills").stat++;
            if (!SteamworksManager.failed) Steamworks.SteamUserStats.AddStat("kills", 1);
        }
    }
    void IncrementDamage(DamageInstanceData data)
    {
        UnlockManager.GetTrackedStatWithName("damage").stat += data.amount;
    }
    //void IncrementDOTDamage(DamageInstanceData data)
    //{
    //    if (data.isDoT && data.target != Player.m_Instance.gameObject) UnlockManager.GetTrackedStatWithName("DoTDamageDealt").stat += data.amount;
    //}
    void IncrementSummonDamage(DamageInstanceData data)
    {
        if (data.abilitySource?.GetComponent<Spell>() == null) return;
        if (!data.abilitySource.GetComponent<Spell>().HasTag(Spell.SpellTag.Summon)) return;

        UnlockManager.GetTrackedStatWithName("summonDamageDealt").stat += data.amount;
    }
    void IncrementBasicSpellDamage(DamageInstanceData data)
    {
        if (data.abilitySource?.GetComponent<Spell>() == null) return;
        if (data.abilitySource != Player.m_Instance.m_ActiveAbility) return;

        UnlockManager.GetTrackedStatWithName("basicSpellDamageDealt").stat += data.amount;
        if (!SteamworksManager.failed) Steamworks.SteamUserStats.AddStat("basic_spell_damage", data.amount);
    }
}
