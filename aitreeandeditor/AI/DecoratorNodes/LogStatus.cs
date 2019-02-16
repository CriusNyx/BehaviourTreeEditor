using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.AI {
    public class LogStatus : Decorator
    {
        public bool enabled = true;
        public string name = "LogStatus";

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
            else
            {
                Debug.Log(name + ": " + current);
            }
            if(current == BehaviourStatus.Call)
                return BehaviourStatus.Running;
            else
                return current;
        }
    }
}