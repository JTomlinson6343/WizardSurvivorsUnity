using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : PauseMenu
{
    [SerializeField] MainMenu m_MainMenuRef;

    public void OnBackButton()
    {
        gameObject.SetActive(false);
        m_MainMenuRef.gameObject.SetActive(true);
        SaveManager.SaveToFile();
    }
}
