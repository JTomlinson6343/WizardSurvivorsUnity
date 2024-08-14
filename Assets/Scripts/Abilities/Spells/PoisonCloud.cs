using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : AOESpawningProtectileSpell
{
    protected override void PlaySound()
    {
        CastSound();
    }
}