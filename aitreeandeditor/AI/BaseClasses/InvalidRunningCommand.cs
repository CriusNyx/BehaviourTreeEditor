using System;

namespace GameSystem.AI
{
    /// <summary>
    /// Exception indicates that node proccessed a running command, voilating the callstack behaviour.
    /// </summary>
    public class InvalidRunningCommand : Exception
    {
        public InvalidRunningCommand()
        {
        }

        public InvalidRunningCommand(string message)
            : base(message)
        {
        }

        public InvalidRunningCommand(string message, Exception inner)
            : base(message, inner)
        {
        }

        public override string Message
        {
            get
            {
                return "Nodes should never prossess AIBehaviourStatus.Running. This behaviour should be handeled by the callstack: " + base.Message;
            }
        }
    }
}