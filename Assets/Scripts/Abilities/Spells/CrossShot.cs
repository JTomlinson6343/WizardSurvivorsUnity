using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : Spell
{
    [SerializeField] float m_Lifetime;
    public override void OnCast()
    {
        if (StateManager.GetCurrentState() == State.PAUSED || StateManager.GetCurrentState() == State.GAME_OVER)  return; 

        base.OnCast();

        ProjectileManager.m_Instance.MultiShot(
            Player.m_Instance.GetStaffTransform().position, m_TotalStats.speed,
            m_TotalStats.amount, this, m_Lifetime);

        AudioManager.m_Instance.PlaySound(4);
    }
}
