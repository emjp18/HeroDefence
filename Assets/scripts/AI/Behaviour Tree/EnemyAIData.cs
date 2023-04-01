using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;

namespace BehaviorTree
{
    public class Idle : Node
    {
        public Idle() : base() { }
        public override NodeState Evaluate()
        {
            //if ((bool)GetData("moveToCenter"))
            //{
            //    ((FlockBehaviourChase)GetData("flockPattern")).RandomW = 0;
            //    ((FlockBehaviourChase)GetData("flockPattern")).TargetW = ((FlockWeights)GetData("flockWeights")).moveToTarget;
            //    SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("hidePoint"),
            //       (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));
            //    state = NodeState.RUNNING;
             
             
            //    if(((Vector2)GetData("movementDirection") - ((Vector2)GetData("hidePoint") - (Vector2)GetData("position")).normalized).magnitude>2)
            //    {
            //        Debug.Log("wrong direction?");
            //    }
            //    return state;
            //}
            //if ((bool)GetData("withinChaseRange"))
            //{
            //    return NodeState.FAILURE;
            //}
           
            //((FlockBehaviourChase)GetData("flockPattern")).RandomW = ((FlockWeights)GetData("flockWeights")).random;
            //((FlockBehaviourChase)GetData("flockPattern")).TargetW = 0;
            //SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
            //   (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));

           

            state = NodeState.RUNNING;
            return state;
        }
    }
    

    public class Death : Node
    {
        public Death() : base() { }

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if ((bool)GetData("dead"))
            {
                
                state = NodeState.SUCCESS;
               
            }

            return state;
        }
    }
    public class FindTarget : Node
    {
        public FindTarget() : base() { }
        public override NodeState Evaluate()
        {
            if((bool)GetData("withinAttackRange"))
            {
                ((AStar2D)GetData("aStar")).ResetPath();
                return NodeState.FAILURE;
            }

            if (((AStar2D)GetData("aStar")).GetPathFound())
            {
                
                var path = ((AStar2D)GetData("aStar")).GetPath();
                if ((int)GetData("pathIndex") == path.Count)
                {

                    SetData("pathIndex", 0);

                    ((AStar2D)GetData("aStar")).ResetPath();

                }
                else
                {

                    SetData("movementDirection", (path[(int)GetData("pathIndex")].pos - (Vector2)GetData("position")).normalized);

                    if (Vector2.Distance((Vector2)GetData("position"), path[(int)GetData("pathIndex")].pos) < (float)GetData("cellSize") * 0.5f)
                    {
                        SetData("pathIndex", (int)GetData("pathIndex") + 1);

                    }

                }
                return NodeState.RUNNING;
            }

            return NodeState.FAILURE;

        }
    }
    
    public class ChaseWithinArea : Node
    {
        public ChaseWithinArea() : base() { }


        public override NodeState Evaluate()
        {
            
            if (!(bool)GetData("withinChaseRange") ||(bool)GetData("moveToCenter"))
            {
                SetData("movementDirection", Vector2.zero);
                return NodeState.FAILURE;
            }



            //((FlockBehaviourChase)GetData("flockPattern")).RandomW = 0;
            //((FlockBehaviourChase)GetData("flockPattern")).TargetW = ((FlockWeights)GetData("flockWeights")).moveToTarget;
            //SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
            //    (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));

           
            state = NodeState.RUNNING;
            return state;
        }

    }
    public class Chase : Node
    {
        public Chase() : base() { }


        public override NodeState Evaluate()
        {

            if ((bool)GetData("withinAttackRange")
              )
            {

                return NodeState.FAILURE;
            }


            //SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
            //    (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));


            state = NodeState.RUNNING;
            return state;
        }

    }
    public class ChaseFindPath : Node
    {
        public ChaseFindPath() : base() { }

    
        public override NodeState Evaluate()
        {
            
            if ((bool)GetData("withinAttackRange"))
            {
                SetData("astarfail", false);
                SetData("searching", false);
                SetData("pathIndex", 0);
                ((AStar2D)GetData("aStar")).ResetPath();
         
                return NodeState.FAILURE;
            }
            if (((AStar2D)GetData("aStar")).GetPathFound())
            {
                var path = ((AStar2D)GetData("aStar")).GetPath();
             
                if ((int)GetData("pathIndex") >= path.Count || (bool)GetData("reset"))
                {
                    SetData("astarfail", false);
                    SetData("searching", false);
                    SetData("pathIndex", 0);
                    ((AStar2D)GetData("aStar")).ResetPath();

                }
                else
                {
                    Vector2 goal = path[(int)GetData("pathIndex")].pos;
                    SetData("movementDirection",
                      ((goal - (Vector2)GetData("position"))).normalized);


                    if (Vector2.Distance((Vector2)GetData("position"), goal) <
                        (float)GetData("cellSize")*0.5f)
                    {
                        SetData("pathIndex", (int)GetData("pathIndex") + 1);
                       
                    }

                }
                
            }
            else if ((bool)GetData("newPath")) 
            {
                SetData("astarfail", false);
                SetData("searching", false);
                SetData("pathIndex", 0);
                ((AStar2D)GetData("aStar")).ResetPath();
                SetData("astarfail", true);
                SetData("searching", true);
                Vector2Int index;
                index = Vector2Int.zero;
                Vector2Int index2 = Vector2Int.zero;

                Utility.GetAIGridIndex((Vector2)GetData("position")+
                    ((((Vector2)GetData("position") -
                        (Vector2)GetData("oldTarget")).normalized) * Utility.GRID_CELL_SIZE*2), ((AStar2D)GetData("aStar")).Quadtree, ref index);
                //Debug.Log(((AiGrid)GetData("grid")).GetCustomGrid()[index.x, index.y].obstacle);
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
    public class ChaseFindPathFlock : Node
    {
        public ChaseFindPathFlock() : base() { }


        public override NodeState Evaluate()
        {

            if ((bool)GetData("withinAttackRange"))
            {
                SetData("checkCollision", false);
                return NodeState.FAILURE;
            }

            if (((AStar2D)GetData("aStar")).GetPathFound())
            {
                SetData("checkCollision", false);
                var path = ((AStar2D)GetData("aStar")).GetPath();
                if ((int)GetData("pathIndex") >= path.Count)
                {

                    SetData("pathIndex", 0);

                    ((AStar2D)GetData("aStar")).ResetPath();


                }
                else
                {

                    //SetData("movementDirection", (path[(int)GetData("pathIndex")].pos - (Vector2)GetData("position")).normalized);
                    SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), path[(int)GetData("pathIndex")].pos,
           (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection"), (Vector2)GetData("leader")));

                    for (int i = (int)GetData("pathIndex"); i < path.Count; i++)
                    {
                        if (Vector2.Distance((Vector2)GetData("position"), path[i].pos) < (float)GetData("cellSize") * 0.5f)
                        {
                            SetData("pathIndex", i + 1);
                            SetData("check", false);

                        }
                    }


                }

            }
            else if ((bool)GetData("newPath"))
            {
                SetData("checkCollision", false);
                Vector2Int index = (Vector2Int)GetData("index");
                index = Vector2Int.zero;
               
                Utility.GetAIGridIndex((Vector2)GetData("obstacleCell"), ((AStar2D)GetData("aStar")).Quadtree, ref index);



                while (((AiGrid)GetData("grid")).GetCustomGrid()[index.x, index.y].obstacle)
                {
                    Vector2 desiredDIr = ((Vector2)GetData("targetPosition") -
                        ((AiGrid)GetData("grid")).GetCustomGrid()[index.x, index.y].pos).normalized;
                    if (desiredDIr.x > 0.5f)//move cells in the direction of the target until one is found that is not an obstacle
                    {
                        if (index.x < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(0))
                            index.x++;
                    }
                    else if (desiredDIr.y > 0.5f)
                    {
                        if (index.y < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(1))
                            index.y++;
                    }
                    else
                    {
                        if (index.y < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(1))
                            index.y++;
                        if (index.x < ((AiGrid)GetData("grid")).GetCustomGrid().GetLength(0))
                            index.x++;
                    }

                }

                SetData("movementDirection", Vector2.zero);

                ((AStar2D)GetData("aStar")).AStarSearch((Vector2)GetData("position"), ((AiGrid)GetData("grid")).GetCustomGrid()[index.x, index.y].pos);

                SetData("newPath", false);


            }
            else
            {
                SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
           (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection"), (Vector2)GetData("leader")));

                SetData("checkCollision", true);
               
            }





            state = NodeState.RUNNING;
            return state;
        }

    }
    public class AttackFast : Node
    {
        public AttackFast() : base() { }
   

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if ((bool)GetData("withinAttackRange"))
            {
                SetData("attacking", true);
                SetData("movementDirection", Vector2.zero);
                state = NodeState.RUNNING;
                //If played animation
                SetData("attacking", false);
            }
                
            
            return state;
        }

    }
    public class MoveForward : Node
    {
        public MoveForward() : base() { }


        public override NodeState Evaluate()
        {
            
            if ((bool)GetData("withinAttackRange"))
            {
                SetData("movementDirection", Vector2.zero);
                return NodeState.FAILURE;
            }
            SetData("movementDirection", (Vector2)GetData("forward"));



            return NodeState.RUNNING;
        }
    }
    public class AttackHeavy : Node
    {
        public AttackHeavy() : base() { }


        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if ((bool)GetData("withinAttackRange"))
            {
                SetData("attacking", true);
                SetData("movementDirection", Vector2.zero);
                state = NodeState.RUNNING;
                //If played animation
                SetData("attacking", false);
            }


            return state;
        }

    }


}