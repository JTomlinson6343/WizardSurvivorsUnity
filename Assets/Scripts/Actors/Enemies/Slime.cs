using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Enemy
{
    public int m_SplitAmount;
    public int m_BabySpawnAmount;
    [SerializeField] float m_BabyBounceSpeed;

    [SerializeField] GameObject m_SlimePrefab;

    void Split()
    {
        for (int i = 0; i < m_BabySpawnAmount; i++)
        {
            GameObject slime = Instantiate(gameObject);
            Slime slimeComponent = slime.GetComponent<Slime>();

            slimeComponent.InitBaby(m_SplitAmount, m_ContactDamage, m_MaxHealth);

            slime.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-1f, 1f) / 1f * m_BabyBounceSpeed, Random.Range(-1f, 1f) / 1f * m_BabyBounceSpeed);
        }
    }

    public void InitBaby(int splitAmount, float damage, float health)
    {
        m_MaxHealth = health / m_BabySpawnAmount;
        m_ContactDamage = damage / m_BabySpawnAmount;
        m_SplitAmount = splitAmount - 1;
        Init();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        if (m_SplitAmount <= 0) return;

        Split();
    }
}
