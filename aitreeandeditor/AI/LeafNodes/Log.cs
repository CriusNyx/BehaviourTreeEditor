using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml.Serialization;

namespace GameSystem.AI
{
    /// <summary>
    /// Debug log the specified message.
    /// </summary>
    public class Log : Leaf
    {
        public string varName;

        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            return BehaviourStatus.Success;
        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            tree.Log(dataContext[varName].ToString());
            return BehaviourStatus.Success;
        }
    }
}