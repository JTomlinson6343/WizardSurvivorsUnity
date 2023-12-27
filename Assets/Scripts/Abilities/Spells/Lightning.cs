using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : Spell
{
    [SerializeField] GameObject m_LightningPrefab;
    public static readonly float kBaseRange = 6f;

    public override void OnCast()
    {
        base.OnCast();
        // If there is an enemy in range
        if (!GameplayManager.GetFurthestEnemyInRange(Player.m_Instance.GetStaffTransform().position, kBaseRange * m_TotalStats.AOE)) return;
        
        GameObject newLightning = Instantiate(m_LightningPrefab);

        newLightning.transform.SetParent(Player.m_Instance.transform);

        newLightning.GetComponent<ParentLightningBolt>().Init(Player.m_Instance.GetStaffTransform().position, this, 0.2f);
    }
}
