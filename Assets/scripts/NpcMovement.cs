using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NpcMovement : MonoBehaviour
{
    /// <summary>
    /// This is where the small cutsceen happens. Bunch of waypoints and animation changes.
    /// </summary>
  
    enum INTROSTATE {MainMenu,Phase1,Phase2,Phase3 };
    [SerializeField]INTROSTATE introstate;
    [SerializeField]private float movespeed = 1f;

    [SerializeField]float timer;
    [SerializeField]float timer2;
    [SerializeField] float texttimer;
    [SerializeField] float npc11Timer;
    private int npc7Count=0;
    private int npc1Count=0;
    private int npc3Count=0;
    private int npc5Count = 0;
    private int npc9Count=0;
    private int npc10Count=0;
    private int npc12Count=0;
    private int npc11Count=0;
    private int textCount;
    public static bool startIntro;
    private bool stepsstart;
    [SerializeField] private int npc8Count = 0;
    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField] private List<Rigidbody2D> rigBod = new List<Rigidbody2D>();

    [SerializeField] private List<Animator> animations = new List<Animator>();

    [SerializeField] private Transform[] waypointBird;
    [SerializeField] private Transform[] waypointNpc11;
    [SerializeField] private Transform[] waypointNpc12;
    [SerializeField] private Transform[] waypointNpc10;
    [SerializeField] private Transform[] waypointNpc9;
    [SerializeField] private Transform[] waypointNpc8;
    [SerializeField] private Transform[] waypointNpc7;
    [SerializeField] private Transform[] waypointNpc6;
    [SerializeField] private Transform[] waypointNpc5;
    [SerializeField] private Transform[] waypointNpc4;
    [SerializeField] private Transform[] waypointNpc3;
    [SerializeField] private Transform[] waypointNpc2;
    [SerializeField] private Transform[] waypointNpc1;
    private int waypointBirdIndex;
    private int waypointNpc11Index;
    private int waypointNpc12Index;
    private int waypointNpc10Index;
    private int waypointNpc9Index;
    private int waypointNpc8Index;
    [SerializeField]private int waypointNpc7Index;
    private int waypointNpc6Index;
    private int waypointNpc5Index;
    private int waypointNpc4Index;
    private int waypointNpc3Index;
    private int waypointNpc2Index;
    private int waypointNpc1Index;

    [SerializeField] TextMeshProUGUI textPart1;
    [SerializeField] TextMeshProUGUI textPart2;

    [SerializeField] private Camera cam;
    void Start()
    {
        stepsstart = true; 
        startIntro = false;
        npc5Count = 0;
        introstate = INTROSTATE.MainMenu;
        timer = 5;
        timer2 = 11.5f;
        animations[6].SetTrigger("FrontIdle");
        animations[8].SetTrigger("SideWalk");
        animations[0].SetTrigger("Idle");
        animations[2].SetTrigger("FrontIdle");
        gameObjects[6].SetActive(false);
        textPart1.gameObject.SetActive(false);
        textPart2.gameObject.SetActive(false);
        gameObjects[13].SetActive(false);
        texttimer = 5;


    }

    void FixedUpdate()
    {
        if(startIntro)
        {
          // Lets you skip Cutsceen and stops the music.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                FindObjectOfType<AudioManager>().Play("DayMusic");
                startIntro= false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                SceneManager.LoadScene("PrototypeV1");
            }
            cam.gameObject.SetActive(true);
            timer -= Time.fixedDeltaTime;
            timer2 -= Time.fixedDeltaTime;
            npc11Timer -= Time.fixedDeltaTime;

            if (timer2 <= 0)
            {

                MoveNpc1();
                MoveNpc3();

            }
            if (timer <= 0)
            {
                gameObjects[6].SetActive(true);
                MoveNpc7();
            }
            if (introstate == INTROSTATE.Phase1 || introstate == INTROSTATE.Phase2 || introstate == INTROSTATE.Phase3)
            {
                texttimer -= Time.fixedDeltaTime;
                MoveNpc11();
                MoveNpc8();
                MoveNpc10();
                MoveNpc9();
            }
            TalkBubble();
            MoveNpc12();
            MoveNpc6();
            MoveNpc5();
            MoveNpc4();
            MoveNpc2();
            MoveBird();
        }

 




    }
    private void MoveBird()
    {
        if (waypointBirdIndex<=waypointBird.Length-1 )
        {

           // moves the targets towards the waypointnumber given in waypointindex;
           // if the target reaches a waypoint it increases the number by 1 and keeps walking untill its done.
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
            gameObjects[0].transform.position = Vector2.MoveTowards(gameObjects[0].transform.position, waypointNpc1[waypointNpc1Index].transform.position, movespeed * Time.deltaTime);

           
            if(introstate == INTROSTATE.Phase1)
            {
                gameObjects[0].transform.Rotate(0, 180, 0);
                introstate = INTROSTATE.Phase2;
            }
            if (gameObjects[0].transform.position == waypointNpc1[waypointNpc1Index].transform.position)
            {
                waypointNpc1Index=0;
                animations[0].SetTrigger("Walk");
                npc1Count=1;
            }
            if(introstate==INTROSTATE.Phase3)
            {
                waypointNpc1Index = 1;
                if(npc1Count==1)
                {
                    animations[0].SetTrigger("Walk");
                    npc1Count=2;
                }
            }
            if(waypointNpc1Index==1 && gameObjects[0].transform.position == waypointNpc1[waypointNpc1Index].transform.position)
            {
                FindObjectOfType<AudioManager>().Play("DayMusic");
                gameObjects[0].SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                SceneManager.LoadScene("PrototypeV1");
              
            }
            if (gameObjects[0].transform.position == waypointNpc1[waypointNpc1Index].transform.position && npc1Count == 1)
            {
                animations[0].SetTrigger("Idle");
                gameObjects[0].transform.Rotate(0, 180, 0);
            }



        }
    }
    private void MoveNpc2()
    {
        if (waypointNpc2Index <= waypointNpc2.Length - 1)
        {
            if(introstate== INTROSTATE.Phase3)
            {
                animations[1].SetTrigger("Walk");
                gameObjects[1].transform.position = Vector2.MoveTowards(gameObjects[1].transform.position, waypointNpc2[waypointNpc2Index].transform.position, movespeed * Time.deltaTime);
                if (gameObjects[1].transform.position == waypointNpc2[waypointNpc2Index].transform.position)
                {
                    gameObjects[1].SetActive(false);
                }
            }
     
        }
    }
    private void MoveNpc3()
    {
        if (waypointNpc3Index <= waypointNpc3.Length - 1)
        {
            gameObjects[2].transform.position = Vector2.MoveTowards(gameObjects[2].transform.position, waypointNpc3[waypointNpc3Index].transform.position, movespeed * Time.deltaTime);

            if(npc3Count== 0) 
            {
                animations[2].SetTrigger("BackWalk");
                npc3Count = 1;
            }
            if (gameObjects[2].transform.position == waypointNpc3[waypointNpc3Index].transform.position && waypointNpc3Index<=1 && npc3Count<3)
            {
                timer2 = 2;
                waypointNpc3Index = 1;
            }
            if(timer2<=0)
            {
            }
            if(introstate== INTROSTATE.Phase3)
            {
               
                waypointNpc3Index = 2;
            }
            if (waypointNpc3Index==2 && npc3Count==4)
            {
                timer2 = 1;
                npc3Count= 5;

            }
            if(waypointNpc3Index == 2&& timer2<=0)
            {
           
                animations[2].SetTrigger("BackWalk");
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
            if(gameObjects[2].transform.position == waypointNpc3[waypointNpc3Index].transform.position && npc3Count==3)
            {
                animations[2].SetTrigger("FrontIdle");
                npc3Count=4;
            }
            if(gameObjects[2].transform.position == waypointNpc3[waypointNpc3Index].transform.position && waypointNpc3Index==2)
            {
                gameObjects[2].SetActive(false);
            }
        }
    }
    private void MoveNpc4()
    {
        if (waypointNpc4Index <= waypointNpc4.Length - 1)
        {
            if (texttimer <= 0 && textCount == 2)
            {
                animations[3].SetTrigger("SideWalk");
                gameObjects[15].transform.position = Vector2.MoveTowards(gameObjects[15].transform.position, waypointNpc4[waypointNpc4Index].transform.position, movespeed * Time.deltaTime);
                if (gameObjects[15].transform.position == waypointNpc4[waypointNpc4Index].transform.position)
                {

                    waypointNpc4Index++;
                }
            }
            if(waypointNpc4Index==1)
            {
                gameObjects[15].SetActive(false);
            }
   
        }
    }
    private void MoveNpc5()
    {
        if (waypointNpc5Index <= waypointNpc5.Length - 1)
        {
            if (npc5Count== 0)
            {
                animations[4].SetTrigger("BackWalk");
                npc5Count = 1;
            }
            if(npc5Count==1 && gameObjects[4].transform.position == waypointNpc5[waypointNpc5Index].transform.position)
            {
                animations[4].SetTrigger("BackIdle");
                npc5Count= 2;
            }
            if(introstate == INTROSTATE.Phase2)
            {
                animations[4].SetTrigger("SideIdle");
            }
            if(npc5Count==2 && waypointNpc5Index==1)
            {
   
                gameObjects[4].SetActive(false);
            }

            gameObjects[4].transform.position = Vector2.MoveTowards(gameObjects[4].transform.position, waypointNpc5[waypointNpc5Index].transform.position, movespeed * Time.deltaTime);
            if (gameObjects[4].transform.position == waypointNpc5[waypointNpc5Index].transform.position)
            {
                if (texttimer <= 0 && textCount == 2)
                {
                    waypointNpc5Index++;
                }
            }

        }
    }
    private void MoveNpc6()
    {
        if (waypointNpc6Index <= waypointNpc5.Length - 1)
        {
            if (texttimer <= 0 && textCount == 2)
            {
                gameObjects[5].transform.position = Vector2.MoveTowards(gameObjects[5].transform.position, waypointNpc6[waypointNpc6Index].transform.position, movespeed * Time.deltaTime);
                if (gameObjects[5].transform.position == waypointNpc6[waypointNpc6Index].transform.position)
                {
                    //waypointNpc6Index++;
                    gameObjects[5].SetActive(false);
                }
                if(waypointNpc6Index== 1)
                {
                }
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
                if (texttimer <= 0 && textCount == 2)
                {
                    waypointNpc8Index=1;
                }
            }
            if (npc8Count == 0 )
            {
                animations[10].SetTrigger("Walk");
                npc8Count = 1;

            }
            if(npc8Count==1 && gameObjects[9].transform.position == waypointNpc8[waypointNpc8Index].transform.position)
            {

                animations[10].SetTrigger("Idle");
            }
            if (npc8Count == 1 && waypointNpc8Index == 1)
            {
                gameObjects[9].SetActive(false);


            }

        }

    }
    private void MoveNpc9()
    {
        if (waypointNpc9Index <= waypointNpc9.Length - 1)
        {
            gameObjects[12].transform.position = Vector2.MoveTowards(gameObjects[12].transform.position, waypointNpc9[waypointNpc9Index].transform.position, movespeed * Time.deltaTime);

            if (gameObjects[12].transform.position == waypointNpc9[waypointNpc9Index].transform.position && introstate==INTROSTATE.Phase3)
            {
                waypointNpc9Index=1;
                animations[12].SetTrigger("Walk");
            }
            if(npc9Count==0)
            {
                gameObjects[12].transform.Rotate(0, 180, 0);
                animations[12].SetTrigger("Walk");
                npc9Count = 1;
            }
            if(gameObjects[12].transform.position == waypointNpc9[waypointNpc9Index].transform.position && waypointNpc9Index==0)
            {
                animations[12].SetTrigger("Idle");
                npc9Count = 2;
            }
            if (gameObjects[12].transform.position == waypointNpc9[waypointNpc9Index].transform.position && waypointNpc9Index == 1)
            {
                gameObjects[12].SetActive(false);
            }
        }

    }
    private void MoveNpc10()
    {
        if (waypointNpc10Index <= waypointNpc10.Length - 1)
        {
            if (introstate == INTROSTATE.Phase3)
            {
                waypointNpc10Index = 1;
          
                animations[9].SetTrigger("SideWalk");

            }
            gameObjects[10].transform.position = Vector2.MoveTowards(gameObjects[10].transform.position, waypointNpc10[waypointNpc10Index].transform.position, movespeed * Time.deltaTime);
            if (gameObjects[10].transform.position == waypointNpc10[waypointNpc10Index].transform.position && introstate==INTROSTATE.Phase3)
            {
                gameObjects[10].SetActive(false);
            }

            if (npc10Count==0)
            {
                animations[9].SetTrigger("SideWalk");
                npc10Count = 1;
            }
            if(waypointNpc10Index==0&&npc10Count==1&& gameObjects[10].transform.position == waypointNpc10[waypointNpc10Index].transform.position)
            {
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
            if(introstate==INTROSTATE.Phase3)
            {
                waypointNpc12Index = 4;
                animations[8].SetTrigger("SideWalk");
            }
            if (waypointNpc12Index == 4 && gameObjects[8].transform.position == waypointNpc12[waypointNpc12Index].transform.position)
            {
                gameObjects[8].SetActive(false);
            }

            if (gameObjects[8].transform.position == waypointNpc12[waypointNpc12Index].transform.position && waypointNpc12Index<=2)
            {
                waypointNpc12Index++;
                
            }
            if(gameObjects[8].transform.position == waypointNpc12[waypointNpc12Index].transform.position && introstate == INTROSTATE.Phase2 || introstate == INTROSTATE.Phase1)
            {
                waypointNpc12Index=3;
            }
            if(waypointNpc12Index==2)
            {
                animations[8].SetTrigger("SideIdle");
            }
            if (waypointNpc12Index == 3)
            {
                if(waypointNpc12Index==3&& gameObjects[8].transform.position == waypointNpc12[waypointNpc12Index].transform.position)
                {
                    animations[8].SetTrigger("SideIdle");
                }
                if(waypointNpc12Index==3 && npc12Count==0)
                {
                animations[8].SetTrigger("SideWalk");
                    npc12Count = 1;

                }
            }
        }
    }
    private void MoveNpc11()
    {
        if (waypointNpc11Index <= waypointNpc11.Length - 1)
        {
            if(npc11Timer<=0)
            {

              


            gameObjects[11].transform.position = Vector2.MoveTowards(gameObjects[11].transform.position, waypointNpc11[waypointNpc11Index].transform.position, movespeed * Time.deltaTime);
            }
            if (gameObjects[11].transform.position == waypointNpc11[waypointNpc11Index].transform.position&& waypointNpc11Index==1)
            {
                gameObjects[11].SetActive(false);
            }
            if(introstate==INTROSTATE.Phase3)
            {
                waypointNpc11Index = 1;
                if (npc11Count == 1)
                {
                animations[11].SetTrigger("SideWalk");
                npc11Timer= 0.5f;
                npc11Count= 2;
                }

            }
            if(npc11Count==0)
            {
                animations[11].SetTrigger("SideWalk");
                npc11Count = 1;
            }
            if(gameObjects[11].transform.position == waypointNpc11[waypointNpc11Index].transform.position)
            {
                animations[11].SetTrigger("SideIdle");
            }
        }
    }
    private void MoveNpc7()
    {
        if(stepsstart)
        {
            FindObjectOfType<AudioManager>().Play("Steps");
            stepsstart = false;
        }
        if(waypointNpc7Index<=waypointNpc7.Length - 1)
        {
            gameObjects[6].transform.position = Vector2.MoveTowards(gameObjects[6].transform.position, waypointNpc7[waypointNpc7Index].transform.position, movespeed * Time.deltaTime);
            
            if (gameObjects[6].transform.position == waypointNpc7[waypointNpc7Index].transform.position && waypointNpc7Index<9)
            {
                waypointNpc7Index++;
          
            }
            if (introstate==INTROSTATE.Phase3)
            {
                animations[6].SetTrigger("BackWalk");
                waypointNpc7Index =10;
            }
            if(waypointNpc7Index==10&& gameObjects[6].transform.position == waypointNpc7[waypointNpc7Index].transform.position)
            {
                gameObjects[6].SetActive(false);
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
            if(waypointNpc7Index==9 && npc7Count == 4)
            {
                introstate = INTROSTATE.Phase1;
             
                npc7Count++;
            }
            if(gameObjects[6].transform.position == waypointNpc7[waypointNpc7Index].transform.position && waypointNpc7Index ==9)
            {
                animations[6].SetTrigger("SideIdle");
            }
        }
    }
    private void TalkBubble()
    { 
        if(introstate== INTROSTATE.Phase2) 
        {
            if(textCount==0)
            {
                FindObjectOfType<AudioManager>().StopPlaying("Steps");
                textPart1.gameObject.transform.Rotate(0, 180, 0);
                textPart2.gameObject.transform.Rotate(0, 180, 0);
                textPart1.gameObject.SetActive(true);
                FindObjectOfType<AudioManager>().Play("gibb");
                textCount =1;
           
            }
            gameObjects[13].SetActive(true);
            gameObjects[14].SetActive(false);
            if (texttimer<=0 && textCount==1)
            {
                FindObjectOfType<AudioManager>().Play("gibb2");
                textPart2.gameObject.SetActive(true);
                textPart1.gameObject.SetActive(false);
                textCount=2;
                texttimer = 5;
            }
            if(texttimer<=0 && textCount==2)
            {
                textPart2.gameObject.SetActive(false);
                gameObjects[13].SetActive(false);
                introstate=INTROSTATE.Phase3;
       
            }
        }
    }
}
