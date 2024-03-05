using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject m_CharMenuRef;
    [SerializeField] GameObject m_OptionsMenuRef;

    [SerializeField] GameObject[] m_Buttons;

    private void Start()
    {
        SaveManager.LoadFromFile(m_CharMenuRef.GetComponent<CharacterMenu>().GetSkillTreeRefs());
    }

    private void Update()
    {

    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        m_CharMenuRef.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void Options()
    {
        gameObject.SetActive(false);
        m_OptionsMenuRef.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Game has been closed");
    }
}
