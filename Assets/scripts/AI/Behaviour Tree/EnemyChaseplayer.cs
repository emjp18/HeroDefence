using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BehaviorTree
{
    public class ChasePlayer : Node
    {
        public ChasePlayer() : base() { }
        public ChasePlayer(List<Node> children) : base(children) { }

        public void SetPlayerTransform(Transform player)
        {
            SetData("Player", player);
        }
        public override NodeState Evaluate()
        {
            
        }

    }
    public class AttackPlayer : Node
    {
        public AttackPlayer() : base() { }
        public AttackPlayer(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {

        }

    }
    public class AttackPlayerFast : Node
    {
        public AttackPlayerFast() : base() { }
        public AttackPlayerFast(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {

        }

    }
    public class AttackPlayerHeavy : Node
    {
        public AttackPlayerHeavy() : base() { }
        public AttackPlayerHeavy(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {

        }

    }
    public class EvadePlayerAttack : Node
    {
        public EvadePlayerAttack() : base() { }
        public EvadePlayerAttack(List<Node> children) : base(children) { }

        public override NodeState Evaluate()
        {

        }

    }
    
        public class EnemyChasePlayerTree : AITree
        {
      
        protected override Node SetupTree()
        {
           Node root = new 
            
        }
    }
}