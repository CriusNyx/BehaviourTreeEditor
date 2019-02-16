using System.Collections;
using System.Collections.Generic;
using System;

namespace GameSystem.AI
{
    /// <summary>
    /// A tree for processing AI commands.
    /// </summary>
    public class AITree
    {
        DataContext dataContext = new DataContext();
        Stack<AINode> callstack = new Stack<AINode>();
        DebugLogger log;

        /// <summary>
        /// Construct a new AI Tree with the specified root.
        /// </summary>
        /// <param name="root"></param>
        public AITree(AINode root, bool debugMode = false)
        {
            SetRoot(root);
            if(debugMode)
                log = new DebugLogger();
            else
                log = new AILogger();
        }

        public object this[string key]
        {
            get
            {
                return dataContext[key];
            }
            set
            {
                dataContext[key] = value;
            }
        }

        /// <summary>
        /// Set a new root for the tree, and clear the callstack.
        /// </summary>
        /// <param name="node"></param>
        public void SetRoot(AINode node)
        {
            callstack = new Stack<AINode>();
            Push(node);
            node = callstack.Peek();
            node.Init(this, dataContext);
        }

        /// <summary>
        /// Push a new node on the call stack, and execute the callstack.
        /// </summary>
        /// <param name="node"></param>
        public void Call(AINode node)
        {
            //Push the new node on the callstack, and fetch the top of the callstack.
            Push(node);
            node = callstack.Peek();

            //Initalize this node, and follow through depending on status.
            var status = node.Init(this, dataContext);
            switch(status)
            {
                case AINode.BehaviourStatus.Success: //Call the node.
                    Call(AINode.BehaviourStatus.Call);
                    break;
                case AINode.BehaviourStatus.Failure: //Pop the node off the callstack, and return execution to it's parent.
                    Pop();
                    Call(AINode.BehaviourStatus.Failure);
                    break;
                case AINode.BehaviourStatus.Running: //These status should be impossible durring Initialization.
                case AINode.BehaviourStatus.Call:
                    throw new InvalidOperationException(callstack.Peek().GetType().ToString() + ": Node cannot reutrn Call or Running from Init");
            }
        }

        /// <summary>
        /// Execute the callstack this frame.
        /// </summary>
        /// <param name="status"></param>
        private void Call(AINode.BehaviourStatus status = AINode.BehaviourStatus.Call)
        {
            //Check for callstack error.
            if(callstack.Count == 0)
                throw new InvalidOperationException("Callstack is empty. Check tree logic.");

            //Get the current node on the callstack, and execute it, fetching it's status.
            AINode current = callstack.Peek();
            status = current.Run(this, dataContext, status);
            switch(status)
            {
                //For success and failure, pop the callstack, and execute the next node.
                case AINode.BehaviourStatus.Success:
                case AINode.BehaviourStatus.Failure:
                    Pop();
                    Call(status);
                    break;
                //This status should be impossible.
                case AINode.BehaviourStatus.Call:
                    throw new InvalidOperationException(callstack.Peek().GetType().ToString() + ": Node cannot return the call status");
                case AINode.BehaviourStatus.Running:
                default:
                    //Do nothing.
                    break;
            }
        }

        /// <summary>
        /// Push a new node on the callstack.
        /// </summary>
        /// <param name="node"></param>
        void Push(AINode node)
        {
            callstack.Push(node.Clone());
        }

        /// <summary>
        /// Pop a node off the callstack.
        /// </summary>
        void Pop()
        {
            callstack.Pop();
        }

        public void PopUntil(AINode node)
        {
            try
            {
                while(callstack.Peek() != node)
                {
                    Pop();
                }
                Pop();
            }
            catch
            {

            }
        }

        public void Log(string message)
        {
            log.Log(message);
        }
    }
}