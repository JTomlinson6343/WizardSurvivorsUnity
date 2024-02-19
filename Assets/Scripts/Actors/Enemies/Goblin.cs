using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Goblin : Enemy
{
    [SerializeField] float m_DontStopRange;
    [SerializeField] float m_StopChancePerFrame;
    [SerializeField] float m_StopDuration;

    [SerializeField] float m_DashSpeedModifier;

    [SerializeField] float m_CamoAlphaValue;
    [SerializeField] float m_VanishDuration;

    bool m_Stopped;
    bool m_Dashing;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, m_DontStopRange);
    }

    public override void Update()
    {
        if (StateManager.IsGameplayStopped())
        {
            rb.velocity = Vector3.zero;
            return;
        }
        RespawnCheck();

        Brain();
    }

    void Brain()
    {
        if (!m_Stopped)
        {
            FollowPlayer();
            RandomStop();
        }
    }

    void RandomStop()
    {
        if (m_Dashing) return;

        // If out of range, never stop
        if (Vector2.Distance(transform.position, Player.m_Instance.transform.position) >= m_DontStopRange) return;

        // If random chance does not happen, dont stop.
        if (Random.Range(0f, 1f) > m_StopChancePerFrame) return;

        m_Stopped = true;

        rb.velocity = Vector2.zero;

        m_Animator.SetBool("Idle", true);
        m_Animator.SetBool("Moving", false);

        StartCoroutine(Vanish(m_VanishDuration, false));
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(m_StopDuration);

        m_Speed *= m_DashSpeedModifier;
        m_Animator.speed *= m_DashSpeedModifier;

        m_Stopped = false;
        m_Dashing = true;
        StartCoroutine(Vanish(m_VanishDuration, true));
    }

    private IEnumerator Vanish(float vanishTime, bool isReversed)
    {
        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
        float alpha = 0f;
        float opacity;

        float targetValue;
        float startValue;

        if (isReversed)
        {
            targetValue = 1f;
            startValue = m_CamoAlphaValue;
        }
        else
        {
            targetValue = m_CamoAlphaValue;
            startValue = 1f;
        }

        while (true)
        {
            if (sprite.color.a == targetValue) break;

            // Increment alpha for the lerp
            alpha += 1f / vanishTime;

            // Reduce opacity slowly
            opacity = Mathf.Lerp(startValue, targetValue, alpha * Time.deltaTime);

            sprite.color = new UnityEngine.Color(sprite.color.r, sprite.color.g, sprite.color.b, opacity);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
