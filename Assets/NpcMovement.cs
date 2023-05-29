using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class NpcMovement : MonoBehaviour
{

  
    enum INTROSTATE {MainMenu,Phase1,Phase2,Phase3 };
    INTROSTATE introstate;
    [SerializeField]private float movespeed = 1f;

    [SerializeField]float timer;
    [SerializeField]float timer2;
    private int npc7Count=0;
    private int npc1Count = 0;
    private int npc3Count = 0;
    private int npc10Count;
    [SerializeField] private int npc8Count = 0;
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField] private List<Rigidbody2D> rigBod = new List<Rigidbody2D>();

    [SerializeField] private List<Animator> animations = new List<Animator>();

    [SerializeField] private Transform[] waypointBird;
    [SerializeField] private Transform[] waypointNpc12;
    [SerializeField] private Transform[] waypointNpc10;
    [SerializeField] private Transform[] waypointNpc8;
    [SerializeField] private Transform[] waypointNpc7;
    [SerializeField] private Transform[] waypointNpc5;
    [SerializeField] private Transform[] waypointNpc3;
    [SerializeField] private Transform[] waypointNpc1;
    private int waypointBirdIndex;
    private int waypointNpc12Index;
    private int waypointNpc10Index;
    [SerializeField] private int waypointNpc8Index;
    private int waypointNpc7Index;
    private int waypointNpc5Index;
    private int waypointNpc3Index;
    private int waypointNpc1Index;
   

    void Start()
    {
        npc10Count = 0;
        introstate = INTROSTATE.MainMenu;
        timer = 5;
        timer2 = 11.5f;
        animations[6].SetTrigger("FrontIdle");
        animations[8].SetTrigger("SideWalk");
        animations[4].SetTrigger("BackWalk");
        animations[0].SetTrigger("Idle");
        animations[2].SetTrigger("FrontIdle");
        gameObjects[6].SetActive(false);


    }

    void FixedUpdate()
    {

        timer -= Time.fixedDeltaTime;
        timer2 -= Time.fixedDeltaTime;
        if(timer2<=0)
        {
       
        MoveNpc1();
        MoveNpc3();

        }
        if(timer<=0)
        {
            gameObjects[6].SetActive(true);
            MoveNpc7();
        }
        MoveNpc12();
        MoveNpc5();
        if (introstate == INTROSTATE.Phase1 || introstate == INTROSTATE.Phase2)
        {

            MoveNpc8();
            MoveNpc10();
        }

        MoveBird();
        Debug.Log("npc8Count NUMMER = " + npc8Count);




    }
    private void MoveBird()
    {
        if(waypointBirdIndex<=waypointBird.Length-1 )
        {
            gameObjects[7].transform.position = Vector2.MoveTowards(gameObjects[7].transform.position, waypointBird[waypointBirdIndex].transform.position, movespeed * Time.deltaTime);
            if (gameObjects[7].transform.position == waypointBird[waypointBirdIndex].transform.position)
            {
                waypointBirdIndex++;
            }
            if (waypointBirdIndex==1)
            {
              
                gameObjects[7].SetActive(false);
            }

        }
    }
    private void MoveNpc1()
    {
        if (waypointNpc1Index <= waypointNpc1.Length - 1)
        {
            //Debug.Log("Index pos" + waypointNpc1[waypointNpc1Index].transform.position);
            //Debug.Log("ObjectPos " + gameObjects[0].transform.position);
            gameObjects[0].transform.position = Vector2.MoveTowards(gameObjects[0].transform.position, waypointNpc1[waypointNpc1Index].transform.position, movespeed * Time.deltaTime);

           
            if(introstate == INTROSTATE.Phase1)
            {
       
                gameObjects[0].transform.Rotate(0, 180, 0);
                introstate= INTROSTATE.Phase2;
            }
            if (gameObjects[0].transform.position == waypointNpc1[waypointNpc1Index].transform.position)
            {
                waypointNpc1Index++;
                animations[0].SetTrigger("Walk");
                npc1Count=1;
            }
            if (waypointNpc1Index == 1 && npc1Count == 1)
            {
                animations[0].SetTrigger("Idle");
                gameObjects[0].transform.Rotate(0, 180, 0);
            }



        }
    }
    private void MoveNpc3()
    {
        if (waypointNpc3Index <= waypointNpc3.Length - 1)
        {
            gameObjects[2].transform.position = Vector2.MoveTowards(gameObjects[2].transform.position, waypointNpc3[waypointNpc3Index].transform.position, movespeed * Time.deltaTime);
            //Debug.Log("Cdad" + npc3Count);
            if(npc3Count== 0) 
            {
                animations[2].SetTrigger("BackWalk");
                npc3Count = 1;
            }
            if (gameObjects[2].transform.position == waypointNpc3[waypointNpc3Index].transform.position)
            {
                waypointNpc3Index++;
                timer2 = 2;
            }
            if(waypointNpc3Index==1 && npc3Count==1)
            {
                animations[2].SetTrigger("FrontIdle");
                npc3Count=2;
            }
            if(timer2<=0&&npc3Count==2)
            {

                animations[2].SetTrigger("FrontWalk");
                npc3Count=3;
            }
            if(waypointNpc3Index==2&& npc3Count==3)
            {
                animations[2].SetTrigger("FrontIdle");
                npc3Count=4;
            }
        }
    }
    private void MoveNpc5()
    {
        if (waypointNpc5Index <= waypointNpc5.Length - 1)
        {

            gameObjects[4].transform.position = Vector2.MoveTowards(gameObjects[4].transform.position, waypointNpc5[waypointNpc5Index].transform.position, movespeed * Time.deltaTime);
            if (gameObjects[4].transform.position == waypointNpc5[waypointNpc5Index].transform.position)
            {
                waypointNpc5Index++;
            }
            if (waypointNpc5Index == 1)
            {
               
                animations[4].SetTrigger("BackIdle");
            }

        }
    }
    private void MoveNpc8()
    {
 
        if (waypointNpc8Index <= waypointNpc8.Length - 1)
        {

                gameObjects[9].transform.position = Vector2.MoveTowards(gameObjects[9].transform.position, waypointNpc8[waypointNpc8Index].transform.position, movespeed * Time.deltaTime);

            if (gameObjects[9].transform.position == waypointNpc8[waypointNpc8Index].transform.position)
            {
                waypointNpc8Index++;
            }
            if (npc8Count == 0 && waypointNpc8Index == 0)
            {
                animations[10].SetTrigger("Walk");
                npc8Count = 1;

            }
            if (npc8Count == 1 && waypointNpc8Index == 1)
            {
                animations[10].SetTrigger("Idle");
               
            }

        }

    }
    private void MoveNpc10()
    {
        if (waypointNpc10Index <= waypointNpc10.Length - 1)
        {
            gameObjects[10].transform.position = Vector2.MoveTowards(gameObjects[10].transform.position, waypointNpc10[waypointNpc10Index].transform.position, movespeed * Time.deltaTime);
            if (gameObjects[10].transform.position == waypointNpc10[waypointNpc10Index].transform.position)
            {
                waypointNpc10Index++;
            }
            if (npc10Count==0)
            {
                animations[9].SetTrigger("SideWalk");
                npc10Count = 1;
            }
            if(waypointNpc10Index==1&&npc10Count==1)
            {
                Debug.Log("aimjdoisamdiasnjdi");
                animations[9].SetTrigger("SideIdle");
                npc10Count = 2;
            }

        }

    }


    private void MoveNpc12()
    {

        if (waypointNpc12Index<=waypointNpc12.Length - 1)
        {
            gameObjects[8].transform.position = Vector2.MoveTowards(gameObjects[8].transform.position, waypointNpc12[waypointNpc12Index].transform.position, movespeed * Time.deltaTime);


            if (gameObjects[8].transform.position == waypointNpc12[waypointNpc12Index].transform.position)
            {
                waypointNpc12Index++;
                
            }
            if(waypointNpc12Index==2)
            {
                animations[8].SetTrigger("SideIdle");
            }
        }
    }
    private void MoveNpc7()
    {
        if(waypointNpc7Index<=waypointNpc7.Length - 1)
        {
            gameObjects[6].transform.position = Vector2.MoveTowards(gameObjects[6].transform.position, waypointNpc7[waypointNpc7Index].transform.position, movespeed * Time.deltaTime);

            if(gameObjects[6].transform.position == waypointNpc7[waypointNpc7Index].transform.position)
            {
                waypointNpc7Index++;
            }

            if (waypointNpc7Index == 1 && npc7Count == 0)
            {
                animations[6].SetTrigger("FrontWalk");
                npc7Count++;
            }
            if (waypointNpc7Index == 2 && npc7Count ==1)
            {
                animations[6].SetTrigger("SideWalk");
                npc7Count++;
            }
            if (waypointNpc7Index == 6 && npc7Count == 2)
            {
                animations[6].SetTrigger("FrontWalk");
                npc7Count++;
            }
            if (waypointNpc7Index == 7 && npc7Count ==3)
            {
                animations[6].SetTrigger("SideWalk");
                gameObjects[6].transform.Rotate(0, 180, 0);
                npc7Count++;

            }
            if(waypointNpc7Index==10 && npc7Count == 4)
            {
                introstate = INTROSTATE.Phase1;
           
                animations[6].SetTrigger("SideIdle");
                animations[4].SetTrigger("SideIdle");
              
            }
        }
    }
}
