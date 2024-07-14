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

    private void Start()
    {
        SaveManager.LoadFromFile(m_CharMenuRef.GetComponent<CharacterMenu>().GetSkillTreeRefs());
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        m_CharMenuRef.SetActive(true);
        m_CharMenuRef.GetComponent<Navigator>().Start();
        Time.timeScale = 1.0f;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        m_OptionsMenuRef.SetActive(true);
        m_OptionsMenuRef.GetComponent<Navigator>().Start();
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
