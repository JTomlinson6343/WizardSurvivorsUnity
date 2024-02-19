using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    protected override void OnDeath()
    {
        base.OnDeath();
        AudioManager.m_Instance.PlaySound(12);
    }

    public void CrawlFromGround()
    {
        Animator animator = GetComponent<Animator>();
        PlayMethodAfterAnimation("Uproot", 0.25f, nameof(EndCrawl));
    }

    public override void Update()
    {
        if (m_IsMidAnimation) return;
        base.Update();
    }

    private void EndCrawl()
    {
        m_IsMidAnimation = false;
    }
}
