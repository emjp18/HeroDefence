using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace BehaviorTree
{
    public class Idle : Node
    {
        public Idle() : base() { }
        public override NodeState Evaluate()
        {
            if ((bool)GetData("moveToCenter"))
            {
                ((FlockBehaviourChase)GetData("flockPattern")).RandomW = 0;
                ((FlockBehaviourChase)GetData("flockPattern")).TargetW = ((FlockWeights)GetData("flockWeights")).moveToTarget;
                SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("hidePoint"),
                   (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));
                state = NodeState.RUNNING;
             
             
                if(((Vector2)GetData("movementDirection") - ((Vector2)GetData("hidePoint") - (Vector2)GetData("position")).normalized).magnitude>2)
                {
                    Debug.Log("wrong direction?");
                }
                return state;
            }
            if ((bool)GetData("withinChaseRange"))
            {
                return NodeState.FAILURE;
            }
           
            ((FlockBehaviourChase)GetData("flockPattern")).RandomW = ((FlockWeights)GetData("flockWeights")).random;
            ((FlockBehaviourChase)GetData("flockPattern")).TargetW = 0;
            SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
               (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));

           

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
                return NodeState.FAILURE;
            }

            if (((AStar2D)GetData("aStar")).GetPathFound())
            {

            }

            return NodeState.RUNNING;
        }
    }
    
    public class ChaseWithinArea : Node
    {
        public ChaseWithinArea() : base() { }


        public override NodeState Evaluate()
        {
            
            if (!(bool)GetData("withinChaseRange") ||(bool)GetData("moveToCenter"))
            {
               
                return NodeState.FAILURE;
            }



            ((FlockBehaviourChase)GetData("flockPattern")).RandomW = 0;
            ((FlockBehaviourChase)GetData("flockPattern")).TargetW = ((FlockWeights)GetData("flockWeights")).moveToTarget;
            SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
                (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));

           
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


            SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
                (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));


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
    public class AttackHeavy : Node
    {
        public AttackHeavy() : base() { }


        public override NodeState Evaluate()
        {
            state = NodeState.FAILURE;
            if ((bool)GetData("withinAttackRange")&&(bool)GetData("attackHeavy")&&!(bool)GetData("outOfRange"))
            {
                SetData("attacking", true);
                SetData("movementDirection", Vector2.zero);
                state = NodeState.RUNNING;
                SetData("attacking", false);
            }


            return state;
        }

    }


}