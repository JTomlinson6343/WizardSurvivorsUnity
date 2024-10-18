using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMageMenu : MonoBehaviour
{
    [SerializeField] GameObject m_LeftCharacterPanel;
    [SerializeField] GameObject m_RightCharacterPanel;

    /*
     * 1. Player selects an icon, left or right.
     * 2. The icon darkens the screen and pops up a new panel that lets the player select a character.
     * 3. The icon from 1. is then replaced with the selected character.
     * 4. The skill tree button becomes visible below the icon, allowing the player to open it and spend skill points.
     * 
     * The total skill points are displayed in the middle.
     */

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
