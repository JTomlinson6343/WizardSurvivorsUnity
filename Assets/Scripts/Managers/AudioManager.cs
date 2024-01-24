using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{
    public static AudioManager m_Instance;

    [SerializeField] AudioClip[] m_AudioClips;

    AudioSource[] m_AudioSources = new AudioSource[512];
    AudioSource m_MusicSource;
    float m_CurrentMusicVolume = 1f;

    public static float m_MusicVolume = 1f;
    public static float m_SoundVolume = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < m_AudioSources.Length; i++)
        {
            AudioSource sourceComponent = gameObject.AddComponent<AudioSource>();
            m_AudioSources[i] = sourceComponent;
        }
        AudioSource musicSourceComponent = gameObject.AddComponent<AudioSource>();
        m_MusicSource = musicSourceComponent;
        m_MusicSource.loop = true;

        UpdateMusicVolume();

        PlayMusic(3);
    }

    public void PlaySound(int soundID)
    {
        AudioSource source = GetFreeAudioSource();

        if (source.volume != m_SoundVolume)
            source.volume = m_SoundVolume;

        source.clip = m_AudioClips[soundID];
        source.Play();
    }

    public void PlaySound(int soundID, float volume)
    {
        AudioSource source = GetFreeAudioSource();
        source.volume = volume * m_SoundVolume;
        source.clip = m_AudioClips[soundID];
        source.Play();
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
