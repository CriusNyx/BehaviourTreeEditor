using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using GameSystem.AI;
using UnityEditor;
using System;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace GameSystem.AI.Editor
{
    [Node(false, "AI/Root")]
    public class EditorAIRootNode : EditorAINode
    {
        public override ConnectionKnob Parent
        {
            get
            {
                return null;
            }
        }

        public string treeName = "*";

        [ConnectionKnob("child", Direction.Out, ConnectionCount.Single, NodeSide.Bottom)]
        public ConnectionKnob child;

        public override string Title
        {
            get
            {
                return "Root Node";
            }
        }

        public const string ID = "aiRootNode";
        public override string GetID
        {
            get
            {
                return ID;
            }
        }

        public override void NodeGUI()
        {
            treeName = EditorGUILayout.TextField(treeName);
            if(GUILayout.Button("Get"))
            {
                Debug.Log(GetXML());
            }
        }

        public string GetXML()
        {
            StringWriter sw = new StringWriter();
            try
            {
                Type type = typeof(AINode);
                Type[] subTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsSubclassOf(type))).ToArray();
                XmlSerializer ser = new XmlSerializer(type, subTypes);
                ser.Serialize(sw, GetTreeNode());
                string output = sw.ToString();
                sw.Close();
                return output;
            }
            catch
            {
                sw.Close();
                return "";
            }
        }

        public override AINode GetTreeNode()
        {
            return ((EditorAINode)(child.connection(0).body)).GetTreeNode();
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