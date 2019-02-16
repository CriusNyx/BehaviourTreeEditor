using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;

namespace GameSystem.AI.Editor
{
    [Node(false, "AI/Decorator")]
    public class EditorAIDecoratorNode : EditorAINode
    {
        public const string ID = "AIDecoratorNode";
        public override string GetID
        {
            get
            {
                return ID;
            }
        }

        [ConnectionKnob("parent", Direction.In, ConnectionCount.Single, NodeSide.Top)]
        public ConnectionKnob parent;

        public override ConnectionKnob Parent
        {
            get
            {
                return parent;
            }
        }

        [ConnectionKnob("child", Direction.Out, ConnectionCount.Single, NodeSide.Bottom)]
        public ConnectionKnob child;

        public override AINode GetTreeNode()
        {
            Decorator output = base.GetTreeNode() as Decorator;
            if(child.connected())
            {
                EditorAINode node = child.connection(0).body as EditorAINode;
                output.subNode = node.GetTreeNode();
            }

            return output;
        }

        public override IEnumerable<EditorAINode> GetChildren()
        {
            if(child.connected())
            {
                EditorAINode node = child.connection(0).body as EditorAINode;
                if(node != null)
                    return new EditorAINode[] { node };
            }
            return new EditorAINode[] { };
        }
    }
}