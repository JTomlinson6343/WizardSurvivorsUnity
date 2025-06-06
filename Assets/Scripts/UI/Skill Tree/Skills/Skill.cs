using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SkillID
{
    None,
    FireSpeed,
    FireDebuff,
    FireSpread,
    FireAOE,
    FireDamageBuff,
    FireFlamethrower,
    FireboltAttackSpeed,
    FireboltDamageBuff,
    FirePassiveSpeed,
    LightningCDR,
    LightningDamageBuff,
    LightningAftershock,
    LightningStun,
    LightningStunCDR,
    LightningConeUpgrade,
    LightningSpellCDR,
    LightningDoubleCast,
    IceResistance,
    IceAOE,
    IceDamage,
    IceSlow,
    IceSlowSpread,
    IceFreezeChance,
    IceElementalSkill,
    IceBlizzardSkill,
    GlobalHealth,
    GlobalDamage,
    GlobalCDR,
    GlobalSpeed,
    GlobalResist,
    GlobalXP,
    GlobalAOE,
    GlobalGoldGem,
    GlobalReroll,
    GlobalLowHealthXP,
    GlobalXPOnKill,
    NecroPoisonDamage,
    NecroDarkDamage,
    NecroSummonDamage,
    NecroLifeDrain,
    NecroSkeletonDuration,
    NecroBleed,
    NecroMoreSkeleton,
    NecroBleedDamage,
    NecroRevive,
    NecroTurned,
    PriestShotAmount,
    PriestBasicSpellDamage,
    PriestBasicSpellCrit,
    PriestAllSpellCrit,
    PriestCritDamage,
    PriestCritCDReduce,
    PriestSmite,
    PriestRicochet,
    PriestRadiantDamage
}

[System.Serializable]
public struct SkillData
{
    public SkillID id;
    public int level;
    public int maxLevel;
}

public class Skill : MonoBehaviour
{
    public SkillData m_Data;

    protected bool m_Active;

    public static bool necroBleedEnabled = false;
    public static bool lightningDoubleCastOn = false;
    public static bool bleedDamageSkillActivated = false;
    public static bool reviveAvailable = false;

    public virtual void Init(SkillData data)
    {
        m_Data = data;
    }
    protected void ResetActiveFlag()
    {
        m_Active = false;
    }

    public bool IsActive()
    {
        return m_Active;
    }
}