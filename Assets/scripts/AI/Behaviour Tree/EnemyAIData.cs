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

            if((bool)GetData("withinRange")|| (bool)GetData("withinAttackRange") || (bool)GetData("dead"))
            {
                return NodeState.FAILURE;
            }
            
   
            SetData("movementDirection", Vector2.zero);

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
    
    
    public class ChaseWithinArea : Node
    {
        public ChaseWithinArea() : base() { }


        public override NodeState Evaluate()
        {

            if(!(bool)GetData("withinRange")|| (bool)GetData("withinAttackRange"))
            {
                return NodeState.FAILURE;
            }
           
          


            SetData("movementDirection", ((FlockBehaviourChase)GetData("flockPattern")).CalculateDirection((Vector2)GetData("position"), (Vector2)GetData("targetPosition"),
                (BoxCollider2D)GetData("box"), (Vector2)GetData("velocity"), (Vector2)GetData("movementDirection")));


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
                SetData("movementDirection", Vector2.zero);
                state = NodeState.RUNNING;
            }
                
            
            return state;
        }

    }
    


}