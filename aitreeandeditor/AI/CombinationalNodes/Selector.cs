using System.Collections;
using System.Collections.Generic;

namespace GameSystem.AI
{
    /// <summary>
    /// Runs each node sequencially, until the first node to succeed.
    /// Returns success if any node succeeds, or failure if all nodes fail.
    /// </summary>
    public class Selector : Combinational
    {
        int count = 0;

        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus status)
        {
            switch(status)
            {
                case BehaviourStatus.Call:
                    if(count == subNodes.Count)
                        return BehaviourStatus.Failure;
                    tree.Call(subNodes[count]);
                    return BehaviourStatus.Running;
                case BehaviourStatus.Success:
                    return BehaviourStatus.Success;
                case BehaviourStatus.Failure:
                    count++;
                    return Run(tree, dataContext, BehaviourStatus.Call);
                case BehaviourStatus.Running:
                    throw new InvalidRunningCommand();
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}