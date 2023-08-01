using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_PlayerReference;
    [SerializeField] private GameObject m_EnemyPrefab;
    [SerializeField] private GameObject m_EnemiesFolder;

    private float m_NextSpawn = 0.0f;
    [SerializeField] private float m_SpawnCooldown = 1.0f;
    [SerializeField] private float m_SpawnRadius = 30.0f;

    private Vector3 GetSpawnPosition()
    {
        Vector3 playerPos = m_PlayerReference.transform.position;

        Vector3 spawnPos = playerPos + new Vector3(Random.Range(-m_SpawnRadius, m_SpawnRadius), Random.Range(-m_SpawnRadius, m_SpawnRadius), 0).normalized * m_SpawnRadius;

        return spawnPos;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float now = Time.realtimeSinceStartup;

        if (now > m_NextSpawn)
        {
            m_NextSpawn = now + m_SpawnCooldown;

            GameObject enemy = Instantiate(m_EnemyPrefab);
            enemy.transform.position = GetSpawnPosition();
            enemy.transform.parent = m_EnemiesFolder.transform;
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_PlayerReference.transform.position, m_SpawnRadius);
    }

    public Vector2 GetClosestEnemyPos(Vector2 pos)
    {
        float minDist = Mathf.Infinity;
        Enemy closestEnemy = null;

        // Get enemies
        Enemy[] enemies = GetComponentsInChildren<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            // Calculate distance from enemy
            float dist = Vector2.Distance(pos, enemy.transform.position);
            if (minDist > dist)
            {
                // If enemy is closer than the closest enemy so far, set closest enemy to this
                minDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy.transform.position;
    }

    public Vector2 GetDirectionToEnemy(Vector2 pos)
    {
        return (pos - GetClosestEnemyPos(pos).normalized);
    }
}