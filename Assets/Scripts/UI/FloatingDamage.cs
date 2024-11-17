using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private float m_RiseSpeed;
    [SerializeField] private float m_ShrinkTime;
    [SerializeField] private Vector3 m_StartSize;
    [SerializeField] private Vector3 m_EndSize;

    private float m_Gradient;
    public Color m_Colour;
    public float m_Damage;

    private void Start()
    {
        transform.localScale = m_StartSize; Invoke(nameof(DestroySelf), m_ShrinkTime);
        m_Gradient = (m_StartSize.x - m_EndSize.x) / m_ShrinkTime;
        GetComponent<TextMeshPro>().text = DamageToText();
        GetComponent<TextMeshPro>().color = m_Colour;
    }
    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x > m_EndSize.x)
        {
            // Shrink damage numbers
            transform.localScale -= new Vector3(m_Gradient * Time.deltaTime, m_Gradient * Time.deltaTime);
        }
        float risePerFrame = m_RiseSpeed * Time.deltaTime;

        // Make damage numbers rise
        transform.position = new Vector2(transform.position.x, transform.position.y + risePerFrame);
    }

    private string DamageToText()
    {
        int damage = Mathf.RoundToInt(m_Damage);
        if (damage <= 0) Destroy(gameObject);

        return damage.ToString();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
