using System.Collections;
using UnityEngine;

public class CurseAOE : ConstantDebuffAOE
{
    private void Start()
    {
        m_DebuffData = new CurseDebuff(Debuff.DebuffType.Curse, DamageType.None, 0, 1, 0.5f, gameObject, m_AbilitySource, 0.9f);
    }
}