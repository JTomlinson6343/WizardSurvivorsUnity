using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class FreezeSpell : Spell
{
    [SerializeField] float m_Range;
    [SerializeField] GameObject m_IcePrefab;
    [SerializeField] float m_LineDuration;

    public override void OnCast()
    {
        base.OnCast();
        
        GameObject enemy = GetRandomEnemy();

        if (!enemy)
        {
            SetRemainingCooldown(kCooldownAfterReset);
            return;
        }

        GameObject ice = Instantiate(m_IcePrefab);

        ice.GetComponent<AOEObject>().Init(enemy.transform.position, this, GetTotalStats().duration);

        DisplayLine(enemy.transform.position);

        AudioManager.m_Instance.PlaySound(20);
    }

    void DisplayLine(Vector2 enemyPos)
    {
        LineRenderer line = GetComponent<LineRenderer>();

        line.SetPosition(0, Player.m_Instance.GetStaffTransform().position);
        line.SetPosition(1, enemyPos);
        line.enabled = true;

        Invoke(nameof(RemoveLine), m_LineDuration);
    }

    void RemoveLine()
    {
        GetComponent<LineRenderer>().enabled = false;
    }

    private GameObject GetRandomEnemy()
    {
        List<GameObject> enemies = GameplayManager.GetAllEnemiesInRange(Player.m_Instance.transform.position, m_Range);

        if (enemies.Count <= 0) return null;

        int RandNo = Random.Range(0, enemies.Count);

        return enemies[RandNo];
    }
}
