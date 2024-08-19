using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager m_Instance;

    [SerializeField] AudioClip[] m_AudioClips;

    AudioSource[] m_AudioSources = new AudioSource[64];
    AudioSource m_MusicSource;
    float m_CurrentMusicVolume = 1f;

    public static float m_MusicVolume = 1f;
    public static float m_SoundVolume = 1f;

    static readonly float kXPSoundEffectCooldown = 0.05f;
    static float m_LastXPSoundTime;
    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;

        for (int i = 0; i < m_AudioSources.Length; i++)
        {
            AudioSource sourceComponent = gameObject.AddComponent<AudioSource>();
            m_AudioSources[i] = sourceComponent;
        }
        AudioSource musicSourceComponent = gameObject.AddComponent<AudioSource>();
        m_MusicSource = musicSourceComponent;
        m_MusicSource.loop = true;

        UpdateMusicVolume();
    }

    public void PlaySound(int soundID)
    {
        AudioSource source = GetFreeAudioSource();

        source.pitch = 1f;

        if (source.volume != m_SoundVolume)
            source.volume = m_SoundVolume;

        source.clip = m_AudioClips[soundID];
        source.Play();
    }

    public void PlaySound(int soundID, float volume)
    {
        AudioSource source = GetFreeAudioSource();

        source.pitch = 1f;
        source.volume = volume * m_SoundVolume;
        source.clip = m_AudioClips[soundID];
        source.Play();
    }

    public void PlaySound(int soundID, float volume, float pitch)
    {
        AudioSource source = GetFreeAudioSource();
        source.volume = volume * m_SoundVolume;
        source.clip = m_AudioClips[soundID];
        source.pitch = pitch;
        source.Play();
    }

    public void PlayRandomPitchSound(int soundID, float volume)
    {
        AudioSource source = GetFreeAudioSource();
        source.volume = volume * m_SoundVolume;
        source.clip = m_AudioClips[soundID];
        source.pitch = Random.Range(0.9f, 1.1f);
        source.Play();
    }
    public void PlayRandomPitchSound(int soundID)
    {
        PlayRandomPitchSound(soundID, 1f);
    }

    public void PlayXPSound()
    {
        if (Time.time - m_LastXPSoundTime > kXPSoundEffectCooldown)
        {
            m_LastXPSoundTime = Time.time;
            float pitch = 0.95f + (ProgressionManager.m_Instance.m_CurrentXP / (float)ProgressionManager.m_Instance.m_NextLevelXP) * 0.2f;
            PlaySound(1, 0.3f, pitch);
        }
    }

    public void PlayMusic(int soundID)
    {
        m_MusicSource.volume = m_MusicVolume;
        m_MusicSource.clip = m_AudioClips[soundID];
        m_MusicSource.Play();
    }
    public void PlayMusic(int soundID, float volume)
    {
        m_MusicSource.volume = volume * m_MusicVolume;
        m_CurrentMusicVolume = volume;
        m_MusicSource.clip = m_AudioClips[soundID];
        m_MusicSource.Play();
    }

    public void UpdateMusicVolume()
    {
        m_MusicSource.volume = m_CurrentMusicVolume * m_MusicVolume;
    }

    AudioSource GetFreeAudioSource()
    {
        foreach(AudioSource source in m_AudioSources)
        {
            if (source.isPlaying) continue;

            return source;
        }
        return m_AudioSources[0];
    }
}
