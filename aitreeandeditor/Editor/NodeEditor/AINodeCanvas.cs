using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using System;
using System.Reflection;
using GameSystem.AI;
using System.Linq;
using UnityEditor;
using System.Xml.Serialization;
using System.IO;
using NodeEditorFramework.Standard;

#if UNITY_EDITOR
using MenuFunction = UnityEditor.GenericMenu.MenuFunction;
using MenuFunctionData = UnityEditor.GenericMenu.MenuFunction2;
#else
using MenuFunction = NodeEditorFramework.Utilities.OverlayGUI.CustomMenuFunction;
using MenuFunctionData = NodeEditorFramework.Utilities.OverlayGUI.CustomMenuFunctionData;
#endif

namespace GameSystem.AI.Editor
{
    /// <summary>
    /// A canvas for editing and automatically saving AI Trees
    /// </summary>
    [NodeCanvasType("AI Editor")]
    public class AINodeCanvas : NodeCanvas
    {
        public EditorAIRootNode rootNode;
        private Type[] nodeTypes;

        private string filepath = "";

        protected override void OnCreate()
        {
            base.OnCreate();
            rootNode = nodes.Find(x => x.GetType() == typeof(EditorAIRootNode)) as EditorAIRootNode;
            if(rootNode == null)
                rootNode = Node.Create(EditorAIRootNode.ID, Vector2.zero, this) as EditorAIRootNode;
        }

        protected override void ValidateSelf()
        {
            DynamicInit();
            if(rootNode == null && (rootNode = nodes.Find((Node n) => n.GetID == EditorAIRootNode.ID) as EditorAIRootNode) == null)
                rootNode = Node.Create(EditorAIRootNode.ID, Vector2.zero, this) as EditorAIRootNode;
            base.ValidateSelf();

            if(filepath != "")
            {
                try
                {
                    File.WriteAllText(filepath, rootNode.GetXML());
                }
                catch
                {

                }
            }
        }

        public static void LoadFile(string filepath)
        {
            NodeEditorWindow editor = NodeEditorWindow.editor;
            var canvas = NodeCanvas.CreateCanvas<AINodeCanvas>();
            canvas._LoadFile(filepath);
            editor.canvasCache.SetCanvas(canvas);
        }

        private void _LoadFile(string filepath)
        {
            this.filepath = filepath;
            string xml = File.ReadAllText(filepath);
            LoadXML(xml);
        }

        public void LoadXML(string xml)
        {
            if(rootNode == null && (rootNode = nodes.Find((Node n) => n.GetID == EditorAIRootNode.ID) as EditorAIRootNode) == null)
                rootNode = Node.Create(EditorAIRootNode.ID, Vector2.zero, this) as EditorAIRootNode;

            Debug.Log("Loading");

            XmlSerializer ser = new XmlSerializer(typeof(AINode), nodeTypes);
            StringReader sr = new StringReader(xml);
            try
            {
                object o = ser.Deserialize(sr);
                if(o is AINode)
                {
                    EditorAINode editorNode = LoadNode(o as AINode);
                    editorNode.Parent.ApplyConnection(rootNode.child);

                    LayoutNodes(rootNode, 0, 10, 0, 100);
                    AdjustLayout(rootNode, -rootNode.position);

                    Debug.Log("Done");
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e);
            }
            sr.Close();
        }

        public EditorAINode LoadNode(AINode node)
        {
            if(node is Decorator)
            {
                return LoadDecorator(node as Decorator);
            }
            else if(node is Combinational)
            {
                return LoadCombinational(node as Combinational);
            }
            else if(node is Leaf)
            {
                return LoadLeaf(node as Leaf);
            }
            else
            {
                throw new System.Exception("Not a node");
            }
        }

        public EditorAINode LoadLeaf(Leaf node)
        {
            EditorAILeafNode output = Node.Create(EditorAILeafNode.ID, Vector3.zero) as EditorAILeafNode;
            output.dataType = node.GetType().ToString();
            output.data = new GenericSerializable(output.dataType);
            output.data.SetObject(node, nodeTypes);
            return output;
        }

        public EditorAINode LoadDecorator(Decorator node)
        {
            EditorAIDecoratorNode output = Node.Create(EditorAIDecoratorNode.ID, Vector3.zero) as EditorAIDecoratorNode;
            output.dataType = node.GetType().ToString();
            var clone = node.Clone() as Decorator;
            clone.subNode = null;
            output.data = new GenericSerializable(output.dataType);
            output.data.SetObject(clone, nodeTypes);

            //load child
            EditorAINode child = LoadNode(node.subNode);
            //attach children
            child.Parent.ApplyConnection(output.child);

            return output;
        }

        public EditorAINode LoadCombinational(Combinational node)
        {
            EditorAICombinationalNode output = Node.Create(EditorAICombinationalNode.ID, Vector3.zero) as EditorAICombinationalNode;
            output.dataType = node.GetType().ToString();
            var clone = node.Clone() as Combinational;

            clone.subNodes = new List<AINode>();

            output.data = new GenericSerializable(output.dataType);
            output.data.SetObject(clone, nodeTypes);

            //load children
            foreach(var child in node.subNodes)
            {
                //load
                EditorAINode childNode = LoadNode(child);
                //attach
                output.PushChild(childNode);
            }

            return output;
        }

        public float LayoutNodes(EditorAINode node, float alignment, float padding, float verticalPosition, float verticalPadding)
        {
            float halfPadding = padding / 2;
            float halfVerticalPading = verticalPadding / 2;
            if(node is EditorAICombinationalNode || node is EditorAIDecoratorNode || node is EditorAIRootNode)
            {
                float startAlign = alignment;
                foreach(var child in node.GetChildren())
                {
                    if(child == null)
                        continue;
                    alignment = LayoutNodes(child, alignment, padding, verticalPosition + node.size.y + verticalPadding, verticalPadding);
                }
                node.position = new Vector2((startAlign + alignment) / 2, (verticalPosition + halfVerticalPading)) + Vector2.left * (node.size.x / 2);
                return alignment;
            }
            else
            {
                node.position = Vector2.zero + Vector2.right * (alignment + halfPadding) + Vector2.up * (verticalPosition + halfVerticalPading);
                return alignment + node.size.x + padding;
            }
        }

        public void AdjustLayout(EditorAINode node, Vector2 offset)
        {
            node.position += offset;
            foreach(var child in node.GetChildren())
            {
                AdjustLayout(child, offset);
            }
        }

        private void DynamicInit()
        {
            if(nodeTypes == null)
                nodeTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsSubclassOf(typeof(AINode)) && !y.IsAbstract)).ToArray();
        }

        protected override void FillContextMenuCanvas(
            Action<NodeEditorInputInfo, NodeEditorFramework.Utilities.GenericMenu, ContextType> orig,
            NodeEditorInputInfo inputInfo,
            NodeEditorFramework.Utilities.GenericMenu contextMenu,
            ContextType contextType)
        {
            foreach(var type in nodeTypes)
            {
                string text = GetTypeString(type, typeof(AINode));
                if(type.GetCustomAttributes(typeof(ContextClickOverrideAttribute), false).Length != 0)
                {
                    ContextClickOverrideAttribute attr = type.GetCustomAttributes(typeof(ContextClickOverrideAttribute), false)[0] as ContextClickOverrideAttribute;
                    text = attr.path;
                }
                contextMenu.AddItem(new GUIContent(text.Trim('/')), true, () => AddNode(type, inputInfo.inputPos));
            }
        }

        private void AddNode(Type type, Vector2 position)
        {
            if(type.IsSubclassOf(typeof(Leaf)))
            {
                EditorAILeafNode node = Node.Create(EditorAILeafNode.ID, position) as EditorAILeafNode;
                node.Init(type);
            }
            else if(type.IsSubclassOf(typeof(Decorator)))
            {
                EditorAIDecoratorNode node = Node.Create(EditorAIDecoratorNode.ID, position) as EditorAIDecoratorNode;
                node.Init(type);
            }
            else if(type.IsSubclassOf(typeof(Combinational)))
            {
                EditorAICombinationalNode node = Node.Create(EditorAICombinationalNode.ID, position) as EditorAICombinationalNode;
                node.Init(type);
            }
        }

        private string GetTypeString(Type type)
        {
            return GetTypeString(typeof(System.Object));
        }

        private string GetTypeString(Type type, Type terminatorType)
        {
            if(type == null || type == terminatorType)
                return "";
            else
            {
                string temp = type.ToString();
                return GetTypeString(type.BaseType, terminatorType) + "/" + temp.Substring(temp.LastIndexOf(".") + 1);
            }
        }

        public override void OnBeforeSavingCanvas()
        {
            string output = "";
            foreach(var node in nodes)
            {
                output += node.GetType().ToString() + "\n";
                EditorAIRootNode rootNode = node as EditorAIRootNode;
                if(rootNode != null)
                    SaveNode(rootNode);
            }
            base.OnBeforeSavingCanvas();
        }

        private void SaveNode(EditorAIRootNode node)
        {
            try
            {
                string fileName = node.treeName;
                if(fileName == "*")
                    fileName = Path.GetFileName(saveName);

                XmlSerializer ser = new XmlSerializer(typeof(AINode), nodeTypes);
                StringWriter sw = new StringWriter();
                ser.Serialize(sw, node.GetTreeNode());
                string xml = sw.ToString();
                //Debug.Log(xml);
                sw.Close();
                string dir = Application.dataPath + "/AI/Resources/AITree/";
                if(!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string path = dir + fileName + ".xml";
                if(File.Exists(path))
                    File.Delete(path);
                File.WriteAllText(path, xml);
            }
            catch(Exception e)
            {
                //Debug.LogError(e.ToString());
            }
        }
    }
}