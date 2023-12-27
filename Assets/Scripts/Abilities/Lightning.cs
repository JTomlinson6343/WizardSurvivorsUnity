using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Ability
{
    [SerializeField] GameObject m_LightningPrefab;
    public static readonly float kBaseRange = 6f;

    public override void OnCast()
    {
        base.OnCast();
        if (GameplayManager.GetFurthestEnemyInRange(Player.m_Instance.GetStaffTransform().position, kBaseRange * m_TotalStats.AOE))
        {
            GameObject newLightning = Instantiate(m_LightningPrefab);

            newLightning.GetComponent<LightningBolt>().Init(Player.m_Instance.GetStaffTransform().position, this, 0.4f);
        }
    }
}
