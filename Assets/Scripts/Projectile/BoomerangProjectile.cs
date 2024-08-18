using UnityEngine;

public class BoomerangProjectile : Projectile
{
    private Vector2 m_Dir;
    private float m_Speed = 0;
    private float m_TargetSpeed;
    private bool m_Returning = false;
    // Update is called once per frame
    public override void Init(Vector2 pos, Vector2 dir, float speed, Ability ability, float lifetime)
    {
        m_AbilitySource = ability;

        // Set pos and velocity of bullet
        transform.position = pos;

        m_Dir = dir;

        m_PierceCount = m_AbilitySource.GetTotalStats().pierceAmount;

        StartCoroutine(Utils.DelayedCall(lifetime, () =>
        {
            m_Returning = true;
            m_Speed = 0f;
        }));
        m_Speed = speed/5f;
        m_TargetSpeed = speed;
    }
    void Update()
    {
        m_Speed += 1.5f * m_TargetSpeed * Time.deltaTime;

        if (m_Returning)
        {
            m_Dir = Utils.GetDirectionToGameObject(transform.position, Player.m_Instance.gameObject);
        }
        else
        {
            m_Speed = Mathf.Clamp(m_Speed, 0f, m_TargetSpeed);
        }

        Rigidbody2D rb = transform.GetComponent<Rigidbody2D>();
        rb.velocity = m_Dir * m_Speed;
    }

    override public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.isTrigger)
        {
            OnTargetHit(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Player") && collision.isTrigger && m_Returning)
        {
            DestroySelf();
        }
    }
}
