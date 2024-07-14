using System.Collections;
using UnityEngine;

public class PersistentCoroutines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Utils.SelectedAnim());
    }
}
