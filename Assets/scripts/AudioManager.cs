using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEditor;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixerGroup mixerGroup;
    public static AudioManager instance;
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
         // Creating the AudioSource
        foreach(Sound s in sounds)
        {

            DontDestroyOnLoad(gameObject);
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop= s.loop;
            s.source.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroup;

        }

        
    }
    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volume / 2f, s.volume / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitch / 2f, s.pitch / 2f));

        s.source.Stop();
    }

    void Start()
    {
      
    }
    private void Update()
    {


    }

    public void Play(string Name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == Name);
        
        if (s==null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.Play();

    }
}
