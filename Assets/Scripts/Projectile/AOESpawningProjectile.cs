using UnityEditor;
using UnityEngine;

public class AOESpawningProjectile : Projectile
{
    public GameObject aoePrefab;
    public float aoeLifetime;

    override protected void DestroySelf()
    {
        GameObject aoe = Instantiate(aoePrefab);
        aoe.transform.position = transform.position;
        aoe.GetComponent<Projectile>().StartLifetimeTimer(aoeLifetime);
        aoe.GetComponent<Projectile>().m_AbilitySource = m_AbilitySource;
        base.DestroySelf();
    }
}
