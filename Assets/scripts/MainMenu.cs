using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/// <summary>
/// 
// Basic MainMenu
/// </summary>
public class MainMenu : MonoBehaviour
{
    int musicCounter;
    public AudioMixer audioMixer;
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
        FindObjectOfType<AudioManager>().Play("crow");
        musicCounter = 0;

    }
    // changes the string to the class that gets picked and sets to StateNameController Script.
    public void ClassSelection(string test )
    {
        global::StateNameController.ClassName = test;
        
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
        // Increases or decreases volume
        audioMixer.SetFloat("Volume",Mathf.Log10(volume)*20);
        Debug.Log(volume);
    }
    

}
