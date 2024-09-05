using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialData
{
    public string name;
    public string title;
    [TextArea(3, 10)]
    public string text;
}

public class TutorialManager : MonoBehaviour
{
    static TutorialManager m_Instance;
    [SerializeField] RectTransform m_Panel;
    [SerializeField] TextMeshProUGUI m_Title;
    [SerializeField] TextMeshProUGUI m_Text;

    [SerializeField] Image m_DarkenScreen;

    [SerializeField] List<TutorialData> m_Tutorials;
    public static List<string> m_ViewedTutorials = new List<string>();

    static TutorialData GetTutorialWithName(string name) { return m_Instance.m_Tutorials.Find(x => x.name == name); }
    static bool WasTutorialViewed(string name) { return m_ViewedTutorials.Contains(name); }

    private void Awake()
    {
        m_Instance = this;
    }

    public static void DisplayTutorial(string tutorialName)
    {
        if (WasTutorialViewed(tutorialName)) return;

        StateManager.ChangeState(StateManager.State.PAUSED);

        TutorialData data = GetTutorialWithName(tutorialName);

        m_Instance.m_Title.text = data.title;
        m_Instance.m_Text.text = data.text;

        m_Instance.m_Panel.transform.localScale = Vector3.zero;

        LeanTween.scale(m_Instance.m_Panel, Vector3.one, 0.25f).setIgnoreTimeScale(true);
        m_Instance.m_DarkenScreen.enabled = true;
        LeanTween.alpha(m_Instance.m_DarkenScreen.GetComponent<RectTransform>(), 0.75f, 0.25f).setIgnoreTimeScale(true);

        m_ViewedTutorials.Add(tutorialName);
        SaveManager.SaveToFile();
    }

    public void CloseTutorial()
    {
        LeanTween.scale(m_Panel, Vector3.zero, 0.25f).setIgnoreTimeScale(true);
        LeanTween.alpha(m_Instance.m_DarkenScreen.GetComponent<RectTransform>(), 0, 0.25f).setIgnoreTimeScale(true).setOnComplete(() =>
        {
            m_Instance.m_DarkenScreen.enabled = false;
        });

        LeanTween.delayedCall(0.25f, () => { StateManager.UnPause(); }).setIgnoreTimeScale(true);
    }
}
