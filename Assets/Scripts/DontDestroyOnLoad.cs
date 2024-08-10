using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static DontDestroyOnLoad instance = null;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance) Destroy(instance.gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
