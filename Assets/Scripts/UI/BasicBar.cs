using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBar : MonoBehaviour
{
    [SerializeField] Vector2 baseScale;

    [SerializeField] float current;
    [SerializeField] float max;

    [SerializeField] Actor m_Actor;

    private void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (!gameObject.GetComponentInParent<Actor>()) return;
        if (!m_Actor) return;

        BasicBar bar = gameObject.GetComponentInChildren<BasicBar>();

        bar.UpdateSize(m_Actor.GetHealthAsRatio());
    }

    public void UpdateSize(float sizeRatio)
    {
        GetComponent<RectTransform>().localScale = new Vector2(sizeRatio * baseScale.x, baseScale.y);
    }

    public void UpdateSize(float currentValue, float maxValue)
    {
        GetComponent<RectTransform>().localScale = new Vector2(currentValue / maxValue * baseScale.x, baseScale.y);
    }
}
