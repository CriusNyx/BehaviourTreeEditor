using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// If queue stack is a loop list, the node will be removed. Otherwise, behaives the same as StackQueuePop
    /// </summary>
    [ContextClickOverride("AIBehaviourTreeLeafNode/QStack/QStackPopRemove")]
    public class QStackPopRemove : Leaf
    {
        public string qStackName = "";
        public string varName = "";

        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            try
            {
                IQStack<object> qStack = dataContext[qStackName] as IQStack<object>;
                if(qStack is LoopList<object>)
                {
                    dataContext[varName] = ((LoopList<object>)qStack).PopRemove();
                }
                else
                {
                    dataContext[varName] = qStack.Pop();
                }
                return BehaviourStatus.Success;
            }
            catch
            {
                return BehaviourStatus.Failure;
            }
        }
    }
}