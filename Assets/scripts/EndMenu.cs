using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    /// <summary>
    /// EndMenu, Activating the correct image and deactivating player,music etc.
    /// </summary>
    public WaveManager WaveManager;
    public GameObject youWinUI;
    public GameObject youLoseUi;
    public Day_Night_Cycle script;
    public GameObject Player;
    public GameObject Player2;
    public GameObject lvlupScreen;
    public BuildingHp buildingHp;
    public float EndTimer = 30;
    private void Start()
    {
        youWinUI.SetActive(false);
        EndTimer = 30;
    }
    public void Update()
    {
        EndTimer-=Time.fixedDeltaTime;
        YouWin();
        YouLose();
    }
    public void LoadMenu()
    {
        FindObjectOfType<AudioManager>().StopPlaying("NightMusic");
        FindObjectOfType<AudioManager>().StopPlaying("DayMusic");
        FindObjectOfType<AudioManager>().StopPlaying("Steps");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void YouWin()
    {
        if (!WaveManager.EnemyIsAlive() && WaveManager.waveComplete == true && script.daytime ==true && WaveManager.currentRoundCount==5)
        {
            Debug.Log("winSenario");
            youWinUI.SetActive(true);
            WaveManager.nextWave = 0;
            Player.SetActive(false);
            Player2.SetActive(false);
            lvlupScreen.SetActive(false);
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
    }
    public void YouLose()
    {
        if(Player.gameObject.GetComponent<PlayerMovement>().currentHealth<=0 && Player.gameObject.activeSelf==true && EndTimer <= 0)
        {
            youLoseUi.SetActive(true);
            Player.SetActive(false);
            lvlupScreen.SetActive(false);
            FindObjectOfType<AudioManager>().StopPlaying("NightMusic");
            FindObjectOfType<AudioManager>().StopPlaying("DayMusic");
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
        if (Player2.gameObject.GetComponent<PlayerMovement>().currentHealth <= 0 && Player2.gameObject.activeSelf == true && EndTimer <= 0)
        {
            youLoseUi.SetActive(true);
            Player2.SetActive(false);
            lvlupScreen.SetActive(false);
            FindObjectOfType<AudioManager>().StopPlaying("DayMusic");
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
        if(buildingHp.currentHealth<=0)
        {
            youLoseUi.SetActive(true);
            Player2.SetActive(false);
            Player.SetActive(false);
            lvlupScreen.SetActive(false);
            FindObjectOfType<AudioManager>().StopPlaying("DayMusic");
            FindObjectOfType<AudioManager>().StopPlaying("Steps");
        }
    }
}
