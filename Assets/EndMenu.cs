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
    public GameObject Player;
    private void Start()
    {
        youWinUI.SetActive(false);
    }
    public void Update()
    {
        YouWin();
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
        if (!WaveManager.EnemyIsAlive() && WaveManager.waveComplete == true && script.daytime ==false)
        {
            Debug.Log("winSenario");
            youWinUI.SetActive(true);
            WaveManager.nextWave = 0;
            Player.SetActive(false);
        }
    }
}
