using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager m_Instance;

    [SerializeField] AudioClip[] m_AudioClips;

    AudioSource[] m_AudioSources = new AudioSource[256];
    AudioSource m_MusicSource;
    // Start is called before the first frame update
    void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            AudioSource sourceComponent = gameObject.AddComponent<AudioSource>();
            m_AudioSources[i] = sourceComponent;
        }
        AudioSource musicSourceComponent = gameObject.AddComponent<AudioSource>();
        m_MusicSource = musicSourceComponent;
        m_MusicSource.loop = true;

        PlayMusic(3);
    }

    public void PlaySound(int soundID)
    {
        AudioSource source = GetFreeAudioSource();
        source.clip = m_AudioClips[soundID];
        source.Play();

    }

    public void PlayMusic(int soundID)
    {
        m_MusicSource.clip = m_AudioClips[soundID];
        m_MusicSource.Play();
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
