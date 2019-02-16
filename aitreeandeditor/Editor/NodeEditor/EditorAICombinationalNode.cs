using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using System.Linq;

namespace GameSystem.AI.Editor
{
    [Node(false, "AI/Combinational")]
    public class EditorAICombinationalNode : EditorAINode
    {
        [ConnectionKnob("parent", Direction.In, ConnectionCount.Single, NodeSide.Top)]
        public ConnectionKnob parent;
        public const string ID = "aiCombinationalNode";
        public override string GetID
        {
            get
            {
                return ID;
            }
        }

        public override ConnectionKnob Parent
        {
            get
            {
                return parent;
            }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            CreateConnectionKnob(new ConnectionKnobAttribute("Child_" + (dynamicConnectionPorts.Count + 1), Direction.Out, ConnectionCount.Single, NodeSide.Bottom));
        }

        public override void DrawKnobs()
        {
            if(dynamicConnectionPorts.Count(x => !x.connected()) != 1)
            {
                var ports = dynamicConnectionPorts.Where(x => !x.connected()).ToList();
                foreach(var p in ports)
                {
                    dynamicConnectionPorts.Remove(p);
                    DestroyImmediate(p);
                }
                CreateConnectionKnob(new ConnectionKnobAttribute("Child_" + (dynamicConnectionPorts.Count + 1), Direction.Out, ConnectionCount.Single, NodeSide.Bottom));
            }
            int offset = 10;
            foreach(var p in dynamicConnectionPorts)
            {
                ConnectionKnob knob = p as ConnectionKnob;
                if(p == null)
                    continue;
                knob.sidePosition = offset;
                offset += 20;
            }
            base.DrawKnobs();
        }

        public void PushChild(EditorAINode node)
        {
            ConnectionPort knob;
            if(dynamicConnectionPorts.Count == 0)
            {
                knob = CreateConnectionKnob(new ConnectionKnobAttribute("Child_" + (dynamicConnectionPorts.Count), Direction.Out, ConnectionCount.Single, NodeSide.Bottom));
                node.Parent.ApplyConnection(knob);
                return;
            }

            knob = dynamicConnectionPorts[dynamicConnectionPorts.Count - 1];
            if(knob.connected())
            {
                knob = CreateConnectionKnob(new ConnectionKnobAttribute("Child_" + (dynamicConnectionPorts.Count), Direction.Out, ConnectionCount.Single, NodeSide.Bottom));
                node.Parent.ApplyConnection(knob);
                return;
            }
            else
            {
                node.Parent.ApplyConnection(knob);
            }
        }

        public override AINode GetTreeNode()
        {
            var output = base.GetTreeNode();
            Combinational combNode = output as Combinational;
            foreach(var childConnection in dynamicConnectionPorts.Where(x => x.direction == Direction.Out))
            {
                if(!childConnection.connected())
                    continue;
                EditorAINode graphNode = childConnection.connection(0).body as EditorAINode;
                combNode.subNodes.Add(graphNode.GetTreeNode());
            }
            return output;
        }

        public override IEnumerable<EditorAINode> GetChildren()
        {
            List<EditorAINode> output = new List<EditorAINode>();
            foreach(var childConnection in dynamicConnectionPorts.Where(x => x.direction == Direction.Out))
            {
                if(!childConnection.connected())
                    continue;
                EditorAINode graphNode = childConnection.connection(0).body as EditorAINode;
                if(graphNode != null)
                    output.Add(graphNode);
            }
            return output;
        }
    }
}