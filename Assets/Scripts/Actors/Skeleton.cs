using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.m_Instance.PlaySound(12);
    }
}
