using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHub : MonoBehaviour
{
    public static CoroutineHub m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Utils.SelectedAnim());
    }
}
