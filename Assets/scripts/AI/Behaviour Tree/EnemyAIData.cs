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
            if (!(bool)GetData("withinGrid"))
            {
                return NodeState.FAILURE;
            }
           
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                return NodeState.FAILURE;
            }
            if(((AStar2D)GetData("AStar2D")).GetPathFound())
            {
                return NodeState.FAILURE;
            }
            if ((float)GetData("time") < (float)GetData("waitTime"))
            {
                SetData("time", (float)GetData("time") + Time.deltaTime);

            }
            else
            {

                SetData("time", 0.0f);
                SetData("pathindex", 0);
                ((AStar2D)GetData("AStar2D")).ResetPath();
                ((AStar2D)GetData("AStar2D")).AStarSearch((Vector2)GetData("position"), (Vector2)GetData("targetPosition"), 50);
            }

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
            if (((EnemyStats)GetData("stats")).Health<=0)
            {
                state = NodeState.SUCCESS;
               
            }

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
            if ((Vector2)GetData("movementDirection") != Vector2.zero)
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

      
        public override NodeState Evaluate()
        {
           
            if (!(bool)GetData("withinGrid")|| !((AStar2D)GetData("AStar2D")).GetPathFound())
            {
               
                state = NodeState.FAILURE;
              
                return state;
            }
           
            Vector2 targetpos = (Vector2)GetData("targetPosition");
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                SetData("pathindex", 0);
                ((AStar2D)GetData("AStar2D")).ResetPath();

                return NodeState.FAILURE;
            }
            if (((AStar2D)GetData("AStar2D")).GetPathFound())
            {
               
                var path = ((AStar2D)GetData("AStar2D")).GetPath();

                if ((int)GetData("pathindex") >= path.Count)
                {

                    SetData("pathindex", 0);

                    ((AStar2D)GetData("AStar2D")).ResetPath();

                }
                else
                {

                    SetData("movementDirection", (path[(int)GetData("pathindex")].pos - (Vector2)GetData("position")).normalized);

                    if (Vector2.Distance((Vector2)GetData("position"), path[(int)GetData("pathindex")].pos) < (float)GetData("cellsize") * 0.5f)
                    {
                        SetData("pathindex", (int)GetData("pathindex") + 1);

                    }

                }


            }

            if (targetpos != (Vector2)GetData("targetPosition"))
            {
                targetpos = (Vector2)GetData("targetPosition");
                if ((float)GetData("time") < (float)GetData("waitTime"))
                {
                    SetData("time", (float)GetData("time") + Time.deltaTime);

                }
                else
                {

                    SetData("time", 0.0f);
                    SetData("pathindex", 0);
                    ((AStar2D)GetData("AStar2D")).ResetPath();
                    ((AStar2D)GetData("AStar2D")).AStarSearch((Vector2)GetData("position"), (Vector2)GetData("targetPosition"), 50);
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