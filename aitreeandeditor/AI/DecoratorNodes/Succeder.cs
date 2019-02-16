using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// Runs the sub node, but always return a success.
    /// </summary>
    public class Succeder : Decorator
    {
        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            switch(current)
            {
                case BehaviourStatus.Call:
                    tree.Call(subNode);
                    return BehaviourStatus.Running;
                case BehaviourStatus.Success:
                    return BehaviourStatus.Success;
                case BehaviourStatus.Failure:
                    return BehaviourStatus.Success;
                case BehaviourStatus.Running:
                    throw new InvalidRunningCommand();
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}