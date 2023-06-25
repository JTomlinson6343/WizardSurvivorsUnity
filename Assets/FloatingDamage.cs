using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private float m_RiseSpeed;
    [SerializeField] private float m_ShrinkSpeed;
    [SerializeField] private Vector3 m_StartSize;
    [SerializeField] private Vector3 m_EndSize;

    private Vector3 m_Gradient;

    public Color m_Colour;

    private void Start()
    {
        transform.localScale = m_StartSize;
    }
    private void Awake()
    {
        Invoke(nameof(DestroySelf), m_ShrinkSpeed);
        m_Gradient = Vector2.Lerp(m_StartSize, m_EndSize, 1/m_ShrinkSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x > m_EndSize.x)
        {
            transform.localScale -= m_Gradient * Time.deltaTime;
        }
        float risePerFrame = m_RiseSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x, transform.position.y + risePerFrame);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
