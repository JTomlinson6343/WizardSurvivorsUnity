using System.Collections;
using UnityEngine;

public class StaticOrb : ConstantDamageAOE
{
    [SerializeField] GameObject m_LightningBoltPrefab;
    readonly float kBaseRange = 3f;

    public override void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        base.Init(pos, dir, speed, ability, lifetime);
        StartCoroutine(ShockEnemies());
    }

    IEnumerator ShockEnemies()
    {
        const float interval = 0.6f;

        while (true)
        {
            GameObject target = Utils.GetFurthestEnemyInRange(transform.position, kBaseRange * m_AbilitySource.GetTotalStats().AOE);
            if (target)
            {
                GameObject bolt = Instantiate(m_LightningBoltPrefab);
                bolt.GetComponent<ParentLightningBolt>().Init(transform.position, m_AbilitySource, 0.2f, kBaseRange);
                bolt.GetComponent<ParentLightningBolt>().m_JumpLimit = 0;
            }

            yield return new WaitForSeconds(interval);
        }
    }
}
