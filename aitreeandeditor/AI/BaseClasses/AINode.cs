using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameSystem.AI
{
    /// <summary>
    /// Parent class for all AI tree nodes.
    /// This class handeles initialization, and running, and provides utility methods for AI nodes.
    /// Note: This class should not be inherrited directly. Instead Leaf, Combinational, or Decorator should be inherrited.
    /// </summary>
    public abstract class AINode
    {
        /// <summary>
        /// Status of an AI call
        /// Success: Indicates success of the last called node, popping it off the callstack.
        /// Failure: Indicates failure of the last called node, popping it off the callstack.
        /// Running: Indicates that the current node is still running, preserving it's status on the callstack.
        /// Call: Called the first time a node is called this frame.
        /// </summary>
        public enum BehaviourStatus
        {
            Success,
            Failure,
            Running,
            Call
        }

        /// <summary>
        /// This is called whenever this AI node executes.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="dataContext"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public abstract BehaviourStatus Run(AITree tree, DataContext dataContext, BehaviourStatus current);

        /// <summary>
        /// This is called when this node is placed on the callstack, before it's executed the first time.
        /// Use this for initialization.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public abstract BehaviourStatus Init(AITree tree, DataContext dataContext);

        /// <summary>
        /// Used to fetch enumerators, allowing utility classes to traverse the AI tree.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<AINode> GetEnumerator();

        /// <summary>
        /// Clones the nodes, so that it can be placed on the callstack.
        /// </summary>
        /// <returns></returns>
        public AINode Clone()
        {
            return base.MemberwiseClone() as AINode;
        }

        /// <summary>
        /// Loads a node with the specified resource path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AINode Load(string path)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Parent class for all AI Leaf nodes.
    /// </summary>
    public abstract class Leaf : AINode
    {
        public override IEnumerator<AINode> GetEnumerator()
        {
            yield break;
        }
    }

    /// <summary>
    /// Parent class for all AI decorator nodes.
    /// A decorator is a node which has only one child.
    /// </summary>
    public abstract class Decorator : AINode
    {
        public AINode subNode;

        public override IEnumerator<AINode> GetEnumerator()
        {
            yield return subNode;
        }
    }

    /// <summary>
    /// Parent node for AI combinational nodes
    /// A combinational node is any node which has multiple chihldren.
    /// </summary>
    public abstract class Combinational : AINode
    {
        public List<AINode> subNodes = new List<AINode>();

        public override IEnumerator<AINode> GetEnumerator()
        {
            foreach(var sub in subNodes)
            {
                yield return sub;
            }
        }
    }
}