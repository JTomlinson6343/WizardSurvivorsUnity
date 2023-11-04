using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEObject : Projectile
{
    protected override void OnEnemyHit(GameObject enemy)
    {
        DamageEnemy(enemy);
    }
}