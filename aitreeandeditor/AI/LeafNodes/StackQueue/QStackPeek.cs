﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.AI
{
    [ContextClickOverride("AIBehaviourTreeLeafNode/QStack/QStackPeek")]
    public class QStackPeek : Leaf
    {
        public string qStackName;
        public string varName;

        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            try
            {
                IQStack<object> qStack = dataContext[qStackName] as IQStack<object>;
                dataContext[varName] = qStack.Peek();
                return BehaviourStatus.Success;
            }
            catch
            {
                return BehaviourStatus.Failure;
            }
        }
    }
}