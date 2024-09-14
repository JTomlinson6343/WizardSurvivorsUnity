using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Scrollbar m_MusicSlider;
    [SerializeField] Scrollbar m_SoundSlider;

    private void Awake()
    {
        InitPauseMenu();
    }
    public void InitPauseMenu()
    {
        m_MusicSlider.value = AudioManager.m_MusicVolume;
        m_SoundSlider.value = AudioManager.m_SoundVolume;
    }

    public void ToggleMenu(bool on)
    {
        gameObject.SetActive(on);
        GetComponent<Navigator>().Start();
    }

    public void OnAutoFireValueChanged(bool value)
    {
        if (Player.m_AutoFire == value) return;
        Player.m_AutoFire = value;

        if (!Player.m_Instance) return;

        Player.m_Instance.ToggleAutoFire();
    }
    public void OnMusicVolumeValueChanged(float value)
    {
        AudioManager.m_MusicVolume = value;

        if (!AudioManager.m_Instance) return;

        AudioManager.m_Instance.UpdateMusicVolume();
    }
    public void OnSoundVolumeValueChanged(float value)
    {
        AudioManager.m_SoundVolume = value;
    }
}
