using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Main Scene");
    }

    public void Options()
    {
        Debug.Log("This will open Options");
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
