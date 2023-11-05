using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillID
{
    None,
    FireSpeed,
    FireDebuff,
    FireSpread,
    FireAOE,
    FireDamageBuff,
    FireFlamethrower
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

    public virtual void Init(SkillData data)
    {
        m_Data = data;
    }
}