using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class GameplayManager
{
    public static List<GameObject> GetAllEnemiesInRange(Vector2 pos, float radius)
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

    public static GameObject GetClosestEnemyInRange(Vector2 pos, float radius)
    {
        if (EnemySpawner.m_Instance == null) return null;

        float minDist = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in GetAllEnemiesInRange(pos, radius))
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
        return closestEnemy;
    }

    //public static GameObject GetFurthestEnemyInRange(Vector2 pos, float radius)
    //{
    //    if (EnemySpawner.m_Instance == null) return null;

    //    float maxDist = 0;
    //    GameObject closestEnemy = null;

    //    foreach (GameObject enemy in GetAllEnemiesInRange(pos, radius))
    //    {
    //        // Calculate distance from enemy
    //        float dist = Vector3.Distance(enemy.transform.position, pos);
    //        if (dist > maxDist)
    //        {
    //            // If enemy is closer than the closest enemy so far, set closest enemy to this
    //            maxDist = dist;
    //            closestEnemy = enemy;
    //        }
    //    }
    //    return closestEnemy;
    //}

    public static Vector2 GetDirectionToEnemy(Vector2 pos, GameObject enemy)
    {
        return ((Vector2)enemy.transform.position - pos).normalized;
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
                return num.ToString();
        }
    }
}