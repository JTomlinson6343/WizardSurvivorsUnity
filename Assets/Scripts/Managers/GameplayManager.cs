using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class GameplayManager
{
    // This class is full of helper functions that involve physics or just general maths calculations
    public static List<GameObject> GetAllEnemiesInRange(Vector2 pos, float radius)
    {
        if (EnemyManager.m_Instance == null) return null;

        // Get all enemies
        Enemy[] enemies = EnemyManager.m_Instance.GetComponentsInChildren<Enemy>();

        List<GameObject> outEnemies = new List<GameObject>();
        foreach (Enemy enemy in enemies)
        {
            // Get distance between passed in pos and enemy pos
            float distance = Vector2.Distance(pos, enemy.transform.position);
            if (distance < radius)
            {
                // If enemy is in range, add it to outEnemies list
                outEnemies.Add(enemy.gameObject);
            }
        }

        // Output enemies which are in range
        return outEnemies;
    }

    public static GameObject GetClosestEnemyInRange(Vector2 pos, float radius)
    {
        if (EnemyManager.m_Instance == null) return null;

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

    public static GameObject GetFurthestEnemyInRange(Vector2 pos, float radius)
    {
        if (EnemyManager.m_Instance == null) return null;

        float maxDist = 0;
        GameObject furthestEnemy = null;

        foreach (GameObject enemy in GetAllEnemiesInRange(pos, radius))
        {
            // Calculate distance from enemy
            float dist = Vector3.Distance(enemy.transform.position, pos);
            if (dist > maxDist)
            {
                // If enemy is further than the furthest enemy so far, set furthest enemy to this
                maxDist = dist;
                furthestEnemy = enemy;
            }
        }
        return furthestEnemy;
    }

    public static Vector2 GetDirectionToGameObject(Vector2 pos, GameObject gameObject)
    {
        return ((Vector2)gameObject.transform.position - pos).normalized;
    }

    public static void PointTowards(Vector2 targetPos, GameObject pointingObject)
    {
        // Get direction to point towards
        Vector2 dir = (targetPos - (Vector2)pointingObject.transform.position).normalized;

        // Point towards said direction
        pointingObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
    }

    public static void PointInDirection(Vector2 dir, GameObject pointingObject)
    {
        // Point towards said direction
        pointingObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90);
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