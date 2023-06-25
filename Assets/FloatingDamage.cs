using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private float m_RiseSpeed;
    [SerializeField] private float m_ShrinkTime;
    [SerializeField] private Vector3 m_StartSize;
    [SerializeField] private Vector3 m_EndSize;

    private float m_Gradient;

    public Color m_Colour;

    private void Start()
    {
        transform.localScale = m_StartSize;
    }
    private void Awake()
    {
        Invoke(nameof(DestroySelf), m_ShrinkTime);
        m_Gradient = (m_StartSize.x - m_EndSize.x) / m_ShrinkTime;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x > m_EndSize.x)
        {
            transform.localScale -= new Vector3(m_Gradient * Time.deltaTime, m_Gradient * Time.deltaTime);
        }
        float risePerFrame = m_RiseSpeed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x, transform.position.y + risePerFrame);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
