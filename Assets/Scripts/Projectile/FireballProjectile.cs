using UnityEditor;
using UnityEngine;

public class FireballProjectile : AOESpawningProjectile
{
    override protected void SpawnAOE()
    {
        base.SpawnAOE();
        AudioManager.m_Instance.PlaySound(11);
    }
}
