using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Scrollbar m_MusicSlider;
    [SerializeField] Scrollbar m_SoundSlider;
    [SerializeField] Toggle m_AutoFireToggle;

    private void Start()
    {
        InitPauseMenu();
    }
    public void InitPauseMenu()
    {
        m_MusicSlider.value = AudioManager.m_MusicVolume;
        m_SoundSlider.value = AudioManager.m_SoundVolume;
        m_AutoFireToggle.isOn = Player.m_AutoFire;
    }

    public void OnAutoFireValueChanged(bool value)
    {
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
