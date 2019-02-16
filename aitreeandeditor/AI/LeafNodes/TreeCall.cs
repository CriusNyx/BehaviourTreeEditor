using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GameSystem.AI
{
    /// <summary>
    /// Call a different tree for additional AI processing.
    /// </summary>
    public class TreeCall : Leaf
    {
        public string subTree;
        [XmlIgnore]
        AINode subNode;

        public override BehaviourStatus Init(AITree tree, DataContext dataContext)
        {
            try
            {
                subNode = AIResourceManager.GetTree(subTree);
                if(subNode == null)
                    return BehaviourStatus.Failure;
                else
                    return BehaviourStatus.Success;
            }
            catch
            {
                return BehaviourStatus.Failure;
            }

        }

        public override BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current)
        {
            if(current == BehaviourStatus.Call)
            {
                tree.Call(subNode);
                return BehaviourStatus.Running;
            }
            else
                return current;
        }
    }
}