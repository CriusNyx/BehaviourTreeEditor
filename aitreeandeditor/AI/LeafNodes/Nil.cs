using System.Collections;
using System.Collections.Generic;

namespace GameSystem.AI
{
    /// <summary>
    /// A nil leaf node on the tree.
    /// Always return success.
    /// </summary>
    public class Nil : Leaf
    {
        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            return BehaviourStatus.Success;
        }
    }
}