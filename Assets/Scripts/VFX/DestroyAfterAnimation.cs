using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    public float m_Lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroySelf), m_Lifetime);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
