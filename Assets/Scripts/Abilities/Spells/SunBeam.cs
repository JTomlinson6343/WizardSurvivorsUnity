using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBeam : AOESpawningSpell
{
    public override void Start()
    {
        base.Start();
        m_CastAmount = m_BaseCastAmount;
    }

    protected override void PlaySound()
    {
        CastSound();
    }
}