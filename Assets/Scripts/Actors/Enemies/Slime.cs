using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : Enemy
{
    public int m_SplitAmount;
    public int m_BabySpawnAmount;
    [SerializeField] float m_BabyBounceSpeed;
    public int m_SlimeVariant;

    void Split()
    {
        for (int i = 0; i < m_BabySpawnAmount; i++)
        {
            GameObject slime = EnemyManager.m_Instance.SpawnBabySlime(m_SlimeVariant);
            slime.transform.SetParent(EnemyManager.m_Instance.transform);
            slime.transform.position = transform.position;
            Slime slimeComponent = slime.GetComponent<Slime>();

            EnemyManager.m_Enemies.Add(slimeComponent);

            slimeComponent.InitBaby(m_SplitAmount, m_ContactDamage, m_MaxHealth, transform.localScale);

            slime.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f) / 1f * m_BabyBounceSpeed, Random.Range(-1f, 1f) / 1f * m_BabyBounceSpeed);
        }
    }

    public void InitBaby(int splitAmount, float damage, float health, Vector3 scale)
    {
        m_MaxHealth = health / m_BabySpawnAmount;
        m_ContactDamage = damage / m_BabySpawnAmount;
        m_SplitAmount = splitAmount - 1;
        transform.localScale = new Vector3(scale.x / m_BabySpawnAmount, scale.y / m_BabySpawnAmount, scale.z);
        Init();
    }

    protected override void OnDeath()
    {
        if (m_SplitAmount > 0) Split();

        AudioManager.m_Instance.PlaySound(12);

        m_Animator.Play("Death");

        Invoke(nameof(DelayedDeath), 0.2f);
    }

    void DelayedDeath()
    {
        base.OnDeath();
    }
}
