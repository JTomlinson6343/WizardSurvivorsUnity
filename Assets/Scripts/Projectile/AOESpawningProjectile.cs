using UnityEditor;
using UnityEngine;

public class AOESpawningProjectile : Projectile
{
    public AOEObject aoePrefab;
    public float aoeLifetime;

    private void OnDestroy()
    {
        AOEObject aoe = Instantiate(aoePrefab);
        aoe.StartLifetimeTimer(aoeLifetime);
    }
}
