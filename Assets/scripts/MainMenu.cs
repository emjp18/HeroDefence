using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene("PrototypeV1");
        FindObjectOfType<AudioManager>().StopPlaying("MenuMusic");

    }
    public void ClassSelection(string test )
    {
        StateNameController.testnr = test;
        
    }


    public void QuitGame()
    {
        Application.Quit();
    }

}
