using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OptionsMenu : PauseMenu
{
    [SerializeField] MainMenu m_MainMenuRef;

    public void OnBackButton()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
        m_MainMenuRef.GetComponent<Navigator>().Start();
        SaveManager.SaveToFile();
    }
}
