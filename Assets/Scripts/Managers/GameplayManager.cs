using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameplayManager
{
    public static List<GameObject> GetAllEnemiesInRadius(Vector2 pos, float radius)
    {
        if (EnemySpawner.m_Instance == null) return null;

        Enemy[] enemies = EnemySpawner.m_Instance.GetComponentsInChildren<Enemy>();

        List<GameObject> outEnemies = new List<GameObject>();
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(pos, enemy.transform.position);
            if (distance < radius)
            {
                outEnemies.Append(enemy.gameObject);
            }
        }
        return outEnemies;
    }
}
