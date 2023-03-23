using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;
using static UnityEngine.RuleTile.TilingRuleOutput;


namespace BehaviorTree
{
    public class IDLE_FLOCK : Node
    {
        public IDLE_FLOCK() : base() { }
        public override NodeState Evaluate()
        {
            
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                return NodeState.FAILURE;
            }
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("chaseRange"))
            {
                return NodeState.FAILURE;
            }
            //run idle animations

            state = NodeState.RUNNING;
            return state;
        }
    }
    public class IdleASTAR : Node
    {
        public IdleASTAR() : base() { }
       
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
    public class Flee : Node
    {
        public Flee() : base() { }

        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if (((EnemyStats)GetData("stats")).Health <= (((EnemyStats)GetData("stats")).Health*0.25f))
            {
                state = NodeState.SUCCESS;

            }

            return state;
        }
    }


    public class MoveToChaseRange : Node
    {
        public MoveToChaseRange() : base() { }

        public override NodeState Evaluate()
        {
            if ((bool)GetData("shouldChase"))
            {
                return NodeState.FAILURE;
            }
            SetData("movementDirection", ((Vector2)GetData("targetPosition") - (Vector2)GetData("position")).normalized);

            state = NodeState.SUCCESS;
            return state;


        }

    }

    public class ChaseASTARGrid : Node
    {
        public ChaseASTARGrid() : base() { }

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
    public class ChaseTargetASTAR : Node
    {
        public ChaseTargetASTAR() : base() { }

      
        public override NodeState Evaluate()
        {
           
            if (!(bool)GetData("withinGrid")|| !((AStar2D)GetData("AStar2D")).GetPathFound())
            {
               
                state = NodeState.FAILURE;
              
                return state;
            }
           
         
            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                SetData("pathindex", 0);
                ((AStar2D)GetData("AStar2D")).ResetPath();

                return NodeState.FAILURE;
            }
            if (((AStar2D)GetData("AStar2D")).GetPathFound())
            {
                SetData("targetUpdated", false);
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
            if((bool)GetData("dynamicTarget")|| (bool)GetData("targetUpdated"))
            {
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
    public class ChaseTargetFLOCK : Node
    {
        public ChaseTargetFLOCK() : base() { }


        public override NodeState Evaluate()
        {


            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) > (float)GetData("chaseRange"))
            {
                SetData("shouldChase", false);
                return NodeState.FAILURE;
            }

            if (Vector2.Distance((Vector2)GetData("position"), (Vector2)GetData("targetPosition")) <= (float)GetData("attackRange"))
            {
                SetData("shouldChase", false);
                return NodeState.FAILURE;
            }
            SetData("shouldChase", true);

            SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
                (BoxCollider2D)GetData("box"), (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));
            
            
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