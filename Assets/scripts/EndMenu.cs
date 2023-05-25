using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public WaveManager WaveManager;
    public GameObject youWinUI;
    public Day_Night_Cycle script;
    public PlayerMovement Player;
    private void Start()
    {
        youWinUI.SetActive(false);
    }
    public void Update()
    {
        YouWin();
        YouLose();
        Debug.Log(Player.alive + "player alive");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void YouWin()
    {

        if (!WaveManager.EnemyIsAlive() && WaveManager.waveComplete == true && script.daytime ==true)
        {
            FindObjectOfType<AudioManager>().StopPlaying("NightMusic");
            Debug.Log("winSenario");
            youWinUI.SetActive(true);
            WaveManager.nextWave = 0;
            //Player.SetActive(false);
            Player.alive = 1;
        }
    }
    public void YouLose()
    {
        if(Player.alive==1)
        {
            youWinUI.SetActive(true);
            FindObjectOfType<AudioManager>().StopPlaying("NightMusic");
        }
    }
}
