using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIcon : MonoBehaviour
{
    public Ability displayedAbility;

    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
}
