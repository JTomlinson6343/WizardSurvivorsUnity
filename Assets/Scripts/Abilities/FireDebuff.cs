using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    public GameObject m_FireParticlePrefab;
    private GameObject m_FireEffect;

    public override void Init(float debuffTime, float damage, DamageType damageType, GameObject source, bool percentHealth, int maxStacks)
    {
        base.Init(debuffTime, damage, damageType, source, percentHealth, maxStacks);

        m_FireEffect = Instantiate(m_FireParticlePrefab);
        m_FireEffect.transform.SetParent(gameObject.transform, false);
    }

    protected override void EndDebuff()
    {
        Destroy(m_FireEffect);
        base.EndDebuff();
    }
}
