using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseDead : Spell
{
    GameObject m_SkeletonPrefab;
    // 1. Spawn skeleton where player clicked (autofire places it on the closest enemy)
    // 2. Enemy runs at the closest enemy until it dies
    // 3. When duration is over, the skeleton explodes, dealing damage in an AOE.

    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);
        FriendlySkeleton skeleton = SpawnSkeleton(closestEnemy.transform.position);
        skeleton.TargetEnemy(closestEnemy);
    }

    FriendlySkeleton SpawnSkeleton(Vector2 pos)
    {
        GameObject skeleton = Instantiate(m_SkeletonPrefab);
        skeleton.transform.position = pos;
        FriendlySkeleton fs = skeleton.GetComponent<FriendlySkeleton>();
        fs.CrawlFromGround();

        return fs;
    }
}
