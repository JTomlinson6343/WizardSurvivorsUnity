using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject m_CharMenuRef;
    [SerializeField] GameObject m_OptionsMenuRef;
    [SerializeField] UnlockMenu m_UnlockMenuRef;
    [SerializeField] GameObject m_CreditsMenuRef;

    static Transform m_SelectedButton;

    private void Start()
    {
        SaveManager.LoadFromFile(m_CharMenuRef.GetComponent<CharacterMenu>().GetSkillTreeRefs());
        MultiMageMenu.m_Instance.gameObject.SetActive(false);
        CharacterMenu.m_Instance.gameObject.SetActive(false);
        AudioManager.m_Instance.PlayMusic(27, 0.7f);
    }

    private void Awake()
    {
        GetComponent<Navigator>().Start();
        Time.timeScale = 1.0f;
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        m_CharMenuRef.GetComponent<CharacterMenu>().OpenMenu();
        Time.timeScale = 1.0f;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        m_OptionsMenuRef.SetActive(true);
        m_OptionsMenuRef.GetComponent<Navigator>().Start();
    }

    public void Credits()
    {
        gameObject.SetActive(false);
        m_CreditsMenuRef.SetActive(true);
        m_CreditsMenuRef.GetComponent<Navigator>().Start();
    }

    public void UnlockMenu()
    {
        m_UnlockMenuRef.Open(gameObject);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
        gameObject.SetActive(true);
        GetComponent<MainMenuNavigator2D>().Start();
    }
}
