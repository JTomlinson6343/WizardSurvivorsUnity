using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] CharacterMenu m_CharMenuRef;

    public void StartGame()
    {
        gameObject.SetActive(false);
        m_CharMenuRef.gameObject.SetActive(true);
    }

    public void Options()
    {
        Debug.Log("This will open Options");
    }

    public void QuitButton()
    {
        SaveManager.SaveToFile();
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
