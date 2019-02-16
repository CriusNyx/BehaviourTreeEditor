using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;

namespace GameSystem.AI.Editor
{
    [Node(false, "AI/Leaf")]
    public class EditorAILeafNode : EditorAINode
    {
        public const string ID = "AILeafNode";
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

        public override IEnumerable<EditorAINode> GetChildren()
        {
            return new EditorAINode[] { };
        }
    }
}