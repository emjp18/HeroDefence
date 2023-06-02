using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
//using UnityEditor.SearchService;
using UnityEditor;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 
    // AudioManger, Controlls all the different sounds i add.
    /// </summary>
    public Sound[] sounds;
    public AudioMixerGroup mixerGroup;
    public static AudioManager instance;
    [SerializeField] public List<AudioSource> sources = new List<AudioSource>();
    private int sourceIndex=0;
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
         // Creating the AudioSource, Changing output to mixergroup, Making sure they dont destroy on load to persist through screens.
        foreach(Sound s in sounds)
        {

            DontDestroyOnLoad(gameObject);
            s.source = sources[sourceIndex];
            s.source.clip = s.clip;
            s.source.loop= s.loop;
            s.source.GetComponent<AudioSource>().outputAudioMixerGroup = mixerGroup;
            sourceIndex++;
            DontDestroyOnLoad(s.source);
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
    public void Play(string Name)
    {
        // searches the sound array to find the sound with the correct name and plays it.
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
