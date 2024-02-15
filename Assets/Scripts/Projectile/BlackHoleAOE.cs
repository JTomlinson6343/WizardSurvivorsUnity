using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlackHoleAOE : DebuffAOE
{
    readonly float kPullSpeedConst = 0.01f;
    readonly float kHitboxShinkAmount = 0.3f;
    readonly float kParticlesToAOERatio = 0.5f;
    [SerializeField] float m_SuckSpeed;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerStay2D(collision);

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
