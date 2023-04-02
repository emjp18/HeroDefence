using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BehaviorTree
{
    public class Idle : Node
    {
        public Idle() : base() { }
        public override NodeState Evaluate()
        {
            if ((bool)GetData("withinAttackRange") || !(bool)GetData("idle"))
            {

                return NodeState.FAILURE;
            }
            SetData("movementDirection", Vector2.zero);
            return NodeState.RUNNING;
        }
    }
    public class Attack : Node
    {
        float time = 0;

        public Attack() : base() { }
        public override NodeState Evaluate()
        {
            if (!(bool)GetData("withinAttackRange"))
            {

                return NodeState.FAILURE;
            }
            if ((float)GetData("attackDelay") > time)
            {
                ((Animator)GetData("animator")).SetBool("attacking", false);
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
                ((Animator)GetData("animator")).SetBool("attacking", true);
            }
            return NodeState.RUNNING;
        }
    }
    public class TakeDamage : Node
    {
        public TakeDamage() : base() { }
        public override NodeState Evaluate()
        {
            if ((float)GetData("health") == (float)GetData("oldHealth"))
            {

                return NodeState.FAILURE;
            }

            if ((float)GetData("health") <= 0)
            {
                ((Animator)GetData("animator")).SetBool("dead", true);

            }
            else
            {
                ((Animator)GetData("animator")).SetBool("hurt", true);
                SetData("oldHealth", GetData("health"));
            }

            return NodeState.RUNNING;

        }
    }

    public class Chase : Node
    {
        public Chase() : base() { }
        int pathIndex = 0;
    
        public override NodeState Evaluate()
        {
            
            if ((bool)GetData("withinAttackRange"))
            {
                SetData("movementDirection", Vector2.zero);
                  pathIndex = 0;
                 ((AStar2D)GetData("aStar")).ResetPath();
         
                return NodeState.FAILURE;
            }
            if (((AStar2D)GetData("aStar")).GetPathFound())
            {
                var path = ((AStar2D)GetData("aStar")).GetPath();
             
                if (pathIndex >= path.Count || (bool)GetData("reset"))
                {
                   
                    pathIndex = 0;
                    ((AStar2D)GetData("aStar")).ResetPath();

                }
                else
                {
                    Vector2 goal = path[pathIndex].pos;
                    


                    
                    SetData("movementDirection",
                      ((goal - (Vector2)GetData("position"))).normalized);


                    if (Vector2.Distance((Vector2)GetData("position"), goal) <
                        (float)GetData("cellSize")*0.5f)
                    {
                        pathIndex++;


                    }

                }
                
            }
            else if ((bool)GetData("newPath")) 
            {
           
     
              

                Vector2Int index;
                index = Vector2Int.zero;
                Vector2Int index2 = Vector2Int.zero;

                Utility.GetAIGridIndex((Vector2)GetData("position") +
    ((((Vector2)GetData("position") -
        (Vector2)GetData("oldTarget")).normalized) * Utility.GRID_CELL_SIZE * 1), ((AStar2D)GetData("aStar")).Quadtree, ref index);

                Utility.GetAIGridIndex((Vector2)GetData("position") +
                  ((((Vector2)GetData("oldTarget") -
                        (Vector2)GetData("position")).normalized) * Utility.GRID_CELL_SIZE), ((AStar2D)GetData("aStar")).Quadtree, ref index2);
                int x = 0;
                while (((AiGrid)GetData("grid")).GetCustomGrid()[index2.x, index2.y].obstacle)
                {
                    x++;
                    bool negX = ((Vector2)GetData("oldTarget")).x <
                        ((Vector2)GetData("position")).x;
                    bool negY = ((Vector2)GetData("oldTarget")).y <
                        ((Vector2)GetData("position")).y;


                    Vector2 desiredDIr = ((Vector2)GetData("oldTarget") -
                        (Vector2)GetData("position")).normalized;




                    if (desiredDIr == Vector2.zero)
                    {
                        desiredDIr = Vector2.one;
                    }



                    if (Mathf.Abs(desiredDIr.x) > Mathf.Abs(desiredDIr.y))
                    {
                        if (negX)
                        {
                            if (index2.x > 0)
                                index2.x--;
                        }
                        else
                        {
                            if (index2.x+1 < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(0))
                                index2.x++;
                        }

                    }
                    else if (Mathf.Abs(desiredDIr.x) < Mathf.Abs(desiredDIr.y))
                    {
                        if (negY)
                        {
                            if (index2.y > 0)
                                index2.y--;
                        }
                        else
                        {
                            if (index2.y+1 < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(0))
                                index2.y++;
                        }

                    }
                    else if (Mathf.Abs(desiredDIr.x) == Mathf.Abs(desiredDIr.y))
                    {
                        if (negX)
                        {
                            if (index2.x+1 < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(0))
                                index2.x++;
                        }
                        if (negY)
                        {
                            if (index2.y+1 < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(1))
                                index2.y++;
                        }


                    }
                    else
                    {
                        Debug.Log("??");
                        break;
                    }

                }
               

                    SetData("movementDirection", Vector2.zero);
                ((AStar2D)GetData("aStar")).Reset = false;
                 ((AStar2D)GetData("aStar")).FindPath(
                   index,
                   index2, 200);
               
                SetData("newPath", false);
            }
            else if (!((AStar2D)GetData("aStar")).GetPathFound()&&!(bool)GetData("newPath"))
            {
                SetData("movementDirection",
                    ((((Vector2)GetData("targetPosition")) - (Vector2)GetData("position")).normalized));

            
                    SetData("checkCollision", true);


            }



           


            state = NodeState.RUNNING;
            return state;
        }

    }
   


}