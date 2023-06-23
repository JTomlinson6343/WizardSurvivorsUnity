using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningProjectile : MonoBehaviour
{
    public float speed;     // Degrees the projectile moves per fer

    public float radius;    // Radius from the player

    public float offset;

    float angle;            // Current angle of the projectile

    public void Init()
    {
        angle = offset;
    }

    // Update is called once per frame
    void Update()
    {
        if (angle > 360)
        {
            angle = 0;
        }
        angle += speed * Time.deltaTime * Mathf.Deg2Rad;

        float x = Player.m_Instance.transform.position.x + radius*Mathf.Sin(angle);
        float y = Player.m_Instance.transform.position.y + radius*Mathf.Cos(angle);

        transform.position = new Vector2(x, y);
    }
}
