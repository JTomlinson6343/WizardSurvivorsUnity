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

    static Transform m_SelectedButton;

    private void Start()
    {
        SaveManager.LoadFromFile(m_CharMenuRef.GetComponent<CharacterMenu>().GetSkillTreeRefs());
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
        m_CharMenuRef.SetActive(true);
        m_CharMenuRef.GetComponent<CharacterMenu>().Awake();
        m_CharMenuRef.GetComponent<Navigator>().Start();
        Time.timeScale = 1.0f;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        m_OptionsMenuRef.SetActive(true);
        m_OptionsMenuRef.GetComponent<Navigator>().Start();
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
}
