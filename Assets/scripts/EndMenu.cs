using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public WaveManager WaveManager;
    public GameObject youWinUI;
    public GameObject youLoseUi;
    public Day_Night_Cycle script;
    public GameObject Player;
    public GameObject Player2;
    private void Start()
    {
        youWinUI.SetActive(false);
    }
    public void Update()
    {
        YouWin();
        YouLose();
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
            Debug.Log("winSenario");
            youWinUI.SetActive(true);
            WaveManager.nextWave = 0;
            Player.SetActive(false);
            Player2.SetActive(false);
        }
    }
    public void YouLose()
    {
        if(Player.gameObject.GetComponent<PlayerMovement>().currentHealth<=0 && Player.gameObject.activeSelf==true)
        {
            youLoseUi.SetActive(true);
            Player.SetActive(false);

        }
        if (Player2.gameObject.GetComponent<PlayerMovement>().currentHealth <= 0 && Player2.gameObject.activeSelf == true)
        {
            youLoseUi.SetActive(true);
            Player2.SetActive(false);
        }
    }
}
