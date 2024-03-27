using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public bool isMuted=false; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
       
        foreach (var sound in sounds)
        {
            sound.audioSource=gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip=sound.clip;
            sound.audioSource.loop=sound.loop;
            sound.audioSource.volume=sound.volume;
            sound.audioSource.pitch=sound.pitch;
        }
    }
    
    void Start()
    {
        Play("Home");
    }

    public void Play(string audioName) 
    {
        Sound s = Array.Find(sounds,sound=>sound.Name==audioName);

        if (s==null)
        {
            return;
        }

        s.audioSource.Play();
    }

    public void Stop(string audioName)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == audioName);

        if (s == null)
        {
            return;
        }

        s.audioSource.Stop();
    }
    
    public void ToggleMusic()
    {
        isMuted= !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        AudioListener.pause = isMuted;
    }
}
