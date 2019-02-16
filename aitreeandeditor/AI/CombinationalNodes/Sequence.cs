using System.Collections;
using System.Collections.Generic;

namespace GameSystem.AI
{
    /// <summary>
    /// Executes each sub node in order, until one fails, are all nodes are executed.
    /// Returns success if all nodes return success, or failure if any nodes fails.
    /// </summary>
    public class Sequence : Combinational
    {
        int current = 0;

        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus status)
        {
            switch(status)
            {
                case BehaviourStatus.Call:
                    if(current == subNodes.Count)
                        return BehaviourStatus.Success;
                    tree.Call(subNodes[current]);
                    return BehaviourStatus.Running;
                case BehaviourStatus.Success:
                    current++;
                    return Run(tree, dataContext, BehaviourStatus.Call);
                case BehaviourStatus.Failure:
                    return BehaviourStatus.Failure;
                case BehaviourStatus.Running:
                    throw new InvalidRunningCommand();
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}