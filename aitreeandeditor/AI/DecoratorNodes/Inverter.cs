using System.Collections;
using System.Collections.Generic;

namespace GameSystem.AI
{
    /// <summary>
    /// Returns the reverse of whatever the child node returns.
    /// </summary>
    public class Inverter : Decorator
    {
        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            switch(current)
            {
                case BehaviourStatus.Success:
                    return BehaviourStatus.Failure;
                case BehaviourStatus.Failure:
                    return BehaviourStatus.Success;
                case BehaviourStatus.Call:
                    tree.Call(subNode);
                    return BehaviourStatus.Running;
                case BehaviourStatus.Running:
                    throw new InvalidRunningCommand();
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}