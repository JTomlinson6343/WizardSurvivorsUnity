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
                outEnemies.Add(enemy.gameObject);
            }
        }
        return outEnemies;
    }
    public static Vector2 GetClosestEnemyPos(Vector2 pos)
    {
        if (EnemySpawner.m_Instance == null) return Vector2.negativeInfinity;

        float minDist = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy enemy in EnemySpawner.m_Instance.GetComponentsInChildren<Enemy>())
        {
            // Calculate distance from enemy
            float dist = Vector3.Distance(enemy.transform.position, pos);
            if (dist < minDist)
            {
                // If enemy is closer than the closest enemy so far, set closest enemy to this
                minDist = dist;
                closestEnemy = enemy;
            }
        }
        return closestEnemy.transform.position;
    }

    public static Vector2 GetDirectionToEnemy(Vector2 pos)
    {
        return (GetClosestEnemyPos(pos) - pos).normalized;
    }

    public static string IntToRomanNumeral(int num)
    {
        switch (num)
        {
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            default:
                return "";
        }
    }
}