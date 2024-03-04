using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class IceStorm : ConstantDamageAOE
{
    [SerializeField] float m_SlowSpeedModifier;
    bool m_IsSlowed;

    [SerializeField] GameObject m_Fog;
    [SerializeField] GameObject m_Snow;

    private Vector3 m_FogBaseSize;
    private Vector3 m_SnowBaseSize;

    private void Awake()
    {
        m_FogBaseSize = m_Fog.transform.localScale;
        m_SnowBaseSize = m_Snow.transform.localScale;
    }

    public override void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        base.Init(pos, dir, speed, ability, lifetime);

        transform.localScale = Vector3.one * ability.GetTotalStats().AOE;
        m_Fog.transform.localScale = m_FogBaseSize * ability.GetTotalStats().AOE;
        m_Snow.transform.localScale = m_SnowBaseSize * ability.GetTotalStats().AOE;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;

        base.OnTriggerEnter2D(collision);

        if (m_IsSlowed) return;

        GetComponent<Rigidbody2D>().velocity *= m_SlowSpeedModifier;

        m_IsSlowed = true;
    }
}