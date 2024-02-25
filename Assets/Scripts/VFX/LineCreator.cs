using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCreator : MonoBehaviour
{
    public void Init(Vector2 pos, Vector2 enemyPos, float duration, Color startColor, Color endColor)
    {
        LineRenderer line = GetComponent<LineRenderer>();

        line.startColor = startColor;
        line.endColor = endColor;

        line.SetPosition(0, pos);
        line.SetPosition(1, enemyPos);

        Invoke(nameof(DestroySelf), duration);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
