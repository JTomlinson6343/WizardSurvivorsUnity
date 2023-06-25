using System.Collections;
using System.Collections.Generic;
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
        // Increment the angle
        angle += speed * Time.deltaTime * Mathf.Deg2Rad;

        // Move the projectile
        float x = Player.m_Instance.GetCentrePos().x + radius*Mathf.Sin(angle);
        float y = Player.m_Instance.GetCentrePos().y + radius*Mathf.Cos(angle);

        transform.position = new Vector2(x, y);
    }
}
