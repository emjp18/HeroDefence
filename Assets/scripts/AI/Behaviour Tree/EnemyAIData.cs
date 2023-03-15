using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


namespace BehaviorTree
{
    public class Idle : Node
    {
        public Idle() : base() { }
       
        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;
            return state;
        }
    }





    public class ChaseGrid : Node
    {
        public ChaseGrid() : base() { }

        public override NodeState Evaluate()
        {
            if ((bool)GetData("withinGrid"))
            {
                return NodeState.FAILURE;
            }
            if ((Vector2)GetData("movementDirection")!=Vector2.zero)
            {
                return NodeState.FAILURE;
            }
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                return NodeState.FAILURE;
            }

            SetData("movementDirection", ((Vector2)GetData("gridCenter") - (Vector2)GetData("position")).normalized);

            state = NodeState.SUCCESS;
            return state;
        }

    }
    public class ChaseTarget : Node
    {
        public ChaseTarget() : base() { }

        int pathindex = 0;
        float time = 0.0f;
        public override NodeState Evaluate()
        {
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                SetData("movementDirection", Vector2.zero);
                ((AStar2D)GetData("AStar2D")).ResetPath();
              
                return NodeState.FAILURE;
            }
            if (((AStar2D)GetData("AStar2D")).GetPathFound())
            {
                var path = ((AStar2D)GetData("AStar2D")).GetPath();
                
                if (pathindex >= path.Count)
                {
                    pathindex = 0;
                
                    SetData("movementDirection", Vector2.zero);
                    ((AStar2D)GetData("AStar2D")).ResetPath();

                }
                else
                {
                    
                    SetData("movementDirection", (path[pathindex].pos - (Vector2)GetData("position")).normalized);

                    float distance = Vector2.Distance((Vector2)GetData("position"), path[pathindex].pos);
                    if (distance < (float)GetData("thresholdDistance"))
                    {
                        pathindex++;
                    }
                    

                }
                

            }
           
            if((bool)GetData("dynamicTarget")|| !((AStar2D)GetData("AStar2D")).GetPathFound())
            {
                if ((bool)GetData("withinGrid"))
                {
                    if (time < (float)GetData("waitTime"))
                    {
                        time += Time.deltaTime;
                    }
                    else
                    {
                        time = 0.0f;
                        ((AStar2D)GetData("AStar2D")).ResetPath();
                        ((AStar2D)GetData("AStar2D")).AStarSearch((Vector2)GetData("position"), (Vector2)GetData("targetPosition"));
                        Debug.Log("Searching");
                    }
                }
            }
          
            state = NodeState.RUNNING;
            return state;
        }

    }
    public class Attack: Node
    {
        public Attack() : base() { }
   

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;

            if(Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition"))<=(float)GetData("attackRange"))
            {
                state = NodeState.SUCCESS;
            }
           



            return state;
        }

    }
    public class AttackFast : Node
    {
        public AttackFast() : base() { }
        public AttackFast(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;
            return state;
        }

    }
    public class AttackHeavy : Node
    {
        public AttackHeavy() : base() { }
        public AttackHeavy(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;
            return state;
        }

    }
    public class Evade : Node
    {
        public Evade() : base() { }
        public Evade(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {
            state = NodeState.RUNNING;
            return state;
        }

    }


}