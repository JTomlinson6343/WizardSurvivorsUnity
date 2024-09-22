using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseDead : Spell
{
    public static RaiseDead m_Instance;
    public float m_DurationModifier = 1f;
    public int m_SkeletonsSpawned = 1;

    [SerializeField] GameObject m_SkeletonPrefab;
    [SerializeField] float m_BaseDuration = 5f;
    [SerializeField] float m_SpawnRadius = 1.5f;
    // 1. Spawn skeleton where player clicked (autofire places it on the closest enemy)
    // 2. Enemy runs at the closest enemy until it dies
    private void Awake()
    {
        m_Instance = this;
    }
    public override void OnCast()
    {
        base.OnCast();

        for (int i = 0; i < m_SkeletonsSpawned; i++)
        {
            // Choose a position in a random place in a radius around the player
            Vector2 pos = Player.m_Instance.transform.position + Utils.GetRandomDirectionV3() * Random.Range(1.5f, 2.5f);
            SpawnSkeleton(pos);
        }
    }

    public static FriendlySkeleton SpawnSkeleton(Vector2 pos)
    {
        GameObject skeleton = Instantiate(m_Instance.m_SkeletonPrefab);
        skeleton.transform.position = pos;

        FriendlySkeleton fs = skeleton.GetComponent<FriendlySkeleton>();
        fs.m_AbilitySource = m_Instance;
        fs.Init(m_Instance.m_BaseDuration * m_Instance.m_DurationModifier);

        return fs;
    }
}
