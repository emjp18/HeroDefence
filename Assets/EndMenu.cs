using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    public WaveManager WaveManager;
    public GameObject youWinUI;
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
        if (!WaveManager.EnemyIsAlive() && WaveManager.spawnState == WaveManager.SpawnState.Waiting)
        {
            youWinUI.SetActive(true);
        }
    }
}
