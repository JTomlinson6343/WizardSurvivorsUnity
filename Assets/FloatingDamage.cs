using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    private void Awake()
    {
        Invoke(nameof(DestroySelf),1.5f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
