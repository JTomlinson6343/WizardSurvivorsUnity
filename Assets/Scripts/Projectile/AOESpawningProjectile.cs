using UnityEditor;
using UnityEngine;

public class AOESpawningProjectile : Projectile
{
    public GameObject aoePrefab;
    public float aoeLifetime;

    override protected void DestroySelf()
    {
        //Debug.Log("DEATH");

        // Spawn the aoe upon death of the projectile
        SpawnAOE();
        base.DestroySelf();
    }

    virtual protected void SpawnAOE()
    {
        GameObject aoe = Instantiate(aoePrefab);
        aoe.GetComponent<AOEObject>().Init(transform.position, m_AbilitySource, aoeLifetime);
        aoe.GetComponent<AOEObject>().SetParticles();
    }
}
