using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StaticOrb : ConstantDamageAOE
{
    [SerializeField] GameObject m_LightningBoltPrefab;
    readonly float kBaseRange = 4.5f;

    public override void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        base.Init(pos, dir, speed, ability, lifetime);
        StartCoroutine(ShockEnemies());
        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        if (rb.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
    }

    IEnumerator ShockEnemies()
    {
        const float interval = 0.4f;

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
