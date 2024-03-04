using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlackHoleAOE : ConstantDamageAOE
{
    readonly float kPullSpeedConst = 0.01f;
    readonly float kParticlesToAOERatio = 0.5f;
    [SerializeField] float m_SuckSpeed;

    private List<GameObject> m_AffectedEnemies = new List<GameObject>();
    private readonly int kAffectedEnemiesLimit = 30;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerStay2D(collision);

        if (m_AffectedEnemies.Count >= kAffectedEnemiesLimit && !m_AffectedEnemies.Contains(collision.gameObject)) return;

        if (!m_AffectedEnemies.Contains(collision.gameObject))
            m_AffectedEnemies.Add(collision.gameObject);

        // Pull all enemies in range into the centre
        Vector3 towardsCentre = (transform.position - collision.transform.position).normalized;

        collision.gameObject.transform.position += towardsCentre * kPullSpeedConst * m_SuckSpeed;
    }

    public override void Init(Vector2 pos, Ability ability, float lifetime)
    {
        base.Init(pos, ability, lifetime);

        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        if (!particles) return;

        ShapeModule shape = particles.shape;
        shape.radius = m_AbilitySource.GetTotalStats().AOE * kParticlesToAOERatio;
    }
}
