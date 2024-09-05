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
        m_MainMenuRef.CloseMenu(gameObject);
        SaveManager.SaveToFile();
    }
}
