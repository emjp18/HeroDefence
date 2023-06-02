using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    private int currentXp = 0;
    public int xpToLvl = 10;

    public GameObject buttonCanvas;
    void Start()
    {
        buttonCanvas.SetActive(false);
    }
    public void RecieveXp(int xp)
    {
        currentXp += xp;
        Debug.Log("works");
        if (currentXp >= xpToLvl) 
        {
            LevelUp();
            currentXp= 0;
        
        }
    }
    void LevelUp()
    {
        buttonCanvas.SetActive(true);
    }
}
