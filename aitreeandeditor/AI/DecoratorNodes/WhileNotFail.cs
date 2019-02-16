using System.Collections;
using System.Collections.Generic;

namespace GameSystem.AI
{
    /// <summary>
    /// Loops until the child node returns a filure.
    /// </summary>
    public class WhileNotFile : Decorator
    {
        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            if(current == BehaviourStatus.Failure)
            {
                return BehaviourStatus.Failure;
            }
            else
            {
                if(current == BehaviourStatus.Call)
                    tree.Call(subNode);
                return BehaviourStatus.Running;
            }
        }
    }
}