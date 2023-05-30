using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    int musicCounter;
    public AudioMixer audioMixer;
    Sound s;
    [SerializeField]private Camera cam;
    public void Update()
    {
        if(musicCounter <= 0)
        {
        FindObjectOfType<AudioManager>().Play("MenuMusic");
            musicCounter++;
        }

    }
    public void PlayGame()
    {
        NpcMovement.startIntro = true;
        cam.gameObject.SetActive(false);
        FindObjectOfType<AudioManager>().StopPlaying("MenuMusic");
        musicCounter = 0;

    }
    public void ClassSelection(string test )
    {
        StateNameController.testnr = test;
        
    }
    public void Pressed()
    {
        FindObjectOfType<AudioManager>().Play("Button");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
    public void VolumeSlider(float volume)
    {

        audioMixer.SetFloat("Volume",Mathf.Log10(volume)*20);
        Debug.Log(volume);
    }
    

}
