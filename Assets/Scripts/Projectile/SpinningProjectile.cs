using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpinningProjectile : Projectile
{
    public float speed;     // Degrees the projectile moves per fer

    public float radius;    // Radius from the player

    public float offset;

    float angle;            // Current angle of the projectile

    public void Init()
    {
        angle = offset * Mathf.Deg2Rad;
        Update();
    }

    private void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (StateManager.GetCurrentState() != State.PLAYING) return;
        // Increment the angle
        angle += speed * Time.deltaTime * Mathf.Deg2Rad;

        float sinAngle = Mathf.Sin(angle);
        float cosAngle = Mathf.Cos(angle);

        // Move the projectile
        float x = Player.m_Instance.GetCentrePos().x + radius*sinAngle;
        float y = Player.m_Instance.GetCentrePos().y + radius*cosAngle;

        // Calculate direction to rotate in
        Vector2 dir = new Vector2(sinAngle, cosAngle);

        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan2(-dir.x,dir.y)*Mathf.Rad2Deg-90);

        transform.position = new Vector2(x, y);
    }

    protected override void OnEnemyHit(GameObject enemy)
    {
        if (m_HitEnemies.Contains(enemy)) return;

        m_HitEnemies.Add(enemy);

        StartCoroutine(EndEnemyCooldown(enemy));

        DamageEnemy(enemy);
    }
}
