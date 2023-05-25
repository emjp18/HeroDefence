using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int musicCounter;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("PrototypeV1");
        FindObjectOfType<AudioManager>().StopPlaying("MenuMusic");
        musicCounter= 0;

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

}
