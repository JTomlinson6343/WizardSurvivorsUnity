using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroRevive : Skill
{
    // 1. Play death animation.
    // 2. Small delay
    // 3. Revive the player and Spawn earthquake

    public static NecroRevive m_Instance;

    [SerializeField] GameObject m_QuakePrefab;

    private void Awake()
    {
        m_Instance = this;
    }
    public override void Init(SkillData data)
    {
        base.Init(data);

        reviveAvailable = true;
    }

    public void Revive()
    {
        Player player = Player.m_Instance;

        reviveAvailable = false;

        player.m_IsInvincible = true;
        StateManager.ChangeState(StateManager.State.REVIVING);

        foreach (Animator animator in player.GetComponentsInChildren<Animator>())
        {
            animator.Play("Death");
        }

        LeanTween.delayedCall(2.25f, () =>
        {
            if (StateManager.GetCurrentState() != StateManager.State.REVIVING) return;
            player.m_IsDead = false;
            player.m_IsInvincible = true;
            player.m_KnockbackResist = 1f;
            player.ClearDebuffs();
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            player.PercentHeal(0.25f);
            foreach (Animator animator in player.GetComponentsInChildren<Animator>())
            {
                animator.Play("Revive");
            }
        });

        LeanTween.delayedCall(2.75f, () =>
        {
            if (StateManager.GetCurrentState() == StateManager.State.REVIVING)
            {
                SpawnQuake();
                PlayerManager.m_Instance.StartShake(0.45f, 0.25f);
                StateManager.UnPause();
            }
        });
        LeanTween.delayedCall(4.75f, () =>
        {
            player.m_IsInvincible = false;
            player.m_KnockbackResist = 0f;
        });
    }

    void SpawnQuake()
    {
        Player player = Player.m_Instance;

        GameObject quake = Instantiate(m_QuakePrefab);
        quake.GetComponentInChildren<AOEObject>().Init(
            player.transform.position,
            GetComponent<Ability>(),
            1.2f); ;

        AudioManager.m_Instance.PlaySound(14);
    }
}
