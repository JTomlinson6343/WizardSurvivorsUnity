using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBar : MonoBehaviour
{
    [SerializeField] Vector2 baseScale;

    [SerializeField] float current;
    [SerializeField] float max;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSize(current, max);
    }

    public void UpdateSize(float currentValue, float maxValue)
    {
        GetComponent<RectTransform>().localScale = new Vector2(currentValue / maxValue * baseScale.x, baseScale.y);
    }
}
