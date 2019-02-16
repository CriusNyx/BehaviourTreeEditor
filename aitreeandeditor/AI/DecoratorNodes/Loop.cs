using System.Collections;
using System.Collections.Generic;

namespace GameSystem.AI
{
    /// <summary>
    /// Run an infinate loop of the child.
    /// Once executed, this node will never be popped from the stack, unless an interrupt is processed.
    /// </summary>
    public class Loop : Decorator
    {
        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            if(current == BehaviourStatus.Call)
            {
                tree.Call(subNode);
            }
            return BehaviourStatus.Running;
        }
    }
}