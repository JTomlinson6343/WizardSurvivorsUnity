using UnityEditor;
using UnityEngine;

public class AOESpawningProjectile : Projectile
{
    public GameObject aoePrefab;
    public float aoeLifetime;

    override protected void DestroySelf()
    {
        GameObject aoe = Instantiate(aoePrefab);
        aoe.GetComponent<AOEObject>().Init(transform.position, m_AbilitySource, aoeLifetime);
        base.DestroySelf();
    }
}
