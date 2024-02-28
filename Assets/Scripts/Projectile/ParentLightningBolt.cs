using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentLightningBolt : LightningBolt
{
    List<GameObject> m_ChildBolts = new List<GameObject>();

    public int m_JumpLimit = 3;
    public int m_JumpCount = 0;

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;
        transform.position = pos;
        StartLifetimeTimer(lifetime);
        InitLengthModifier();
        Zap();
    }
    protected override void OnTargetHit(GameObject enemy)
    {
        base.OnTargetHit(enemy);
        LightningJump(enemy);
    }
    protected override void LightningJump(GameObject enemy)
    {
        // If the lightning has already jumped the max number of times, return
        if (m_JumpCount >= m_JumpLimit + m_AbilitySource.GetTotalStats().pierceAmount) return;
        // If there are no other enemies in range, return
        if (GameplayManager.GetAllEnemiesInRange(transform.position, Lightning.kBaseRange * m_AbilitySource.GetTotalStats().AOE).Count <= 1) return;

        GameObject newLightning = Instantiate(m_LightningPrefab);
        newLightning.GetComponent<LightningBolt>().Init(enemy.transform.position, m_AbilitySource, this);
    }

    protected override void DestroySelf()
    {
        foreach (GameObject bolt in m_ChildBolts)
        {
            Destroy(bolt);
        }
        base.DestroySelf();
    }

    public void AddToChildBolts(GameObject gameObject)
    {
        m_ChildBolts.Add(gameObject);
    }
}
 