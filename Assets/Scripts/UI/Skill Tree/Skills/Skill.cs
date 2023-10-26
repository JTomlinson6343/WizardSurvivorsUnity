using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillID
{
    None,
    FireSpeed,
    FireDebuff
}

public class Skill : MonoBehaviour
{
    public SkillID m_ID;

    public virtual void Init() { }
}