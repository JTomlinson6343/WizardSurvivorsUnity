using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : AOESpawningProtectileSpell
{
    protected override void PlaySound()
    {
        AudioManager.m_Instance.PlaySound(16);
    }
}