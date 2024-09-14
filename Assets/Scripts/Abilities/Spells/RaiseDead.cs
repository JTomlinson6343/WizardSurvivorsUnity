using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseDead : Spell
{
    static RaiseDead m_Instance;
    public static float m_DurationModifier = 1f;
    public static int m_SkeletonsSpawned = 1;

    [SerializeField] GameObject m_SkeletonPrefab;
    [SerializeField] float m_BaseDuration = 5f;
    // 1. Spawn skeleton where player clicked (autofire places it on the closest enemy)
    // 2. Enemy runs at the closest enemy until it dies
    private void Awake()
    {
        m_Instance = this;
    }
    public override void OnCast()
    {
        base.OnCast();

        GameObject closestEnemy = Utils.GetClosestEnemyInRange(Player.m_Instance.GetStaffTransform().position, m_DefaultAutofireRange);
        FriendlySkeleton skeleton = SpawnSkeleton(closestEnemy.transform.position);
        skeleton.TargetEnemy(closestEnemy);
    }

    public static FriendlySkeleton SpawnSkeleton(Vector2 pos)
    {
        GameObject skeleton = Instantiate(m_Instance.m_SkeletonPrefab);
        skeleton.transform.position = pos;

        FriendlySkeleton fs = skeleton.GetComponent<FriendlySkeleton>();
        fs.m_AbilitySource = m_Instance;
        fs.Init(m_Instance.m_BaseDuration * m_DurationModifier);

        return fs;
    }
}
