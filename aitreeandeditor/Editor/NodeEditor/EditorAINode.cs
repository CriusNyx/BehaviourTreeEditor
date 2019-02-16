using System;
using GameSystem.AI;
using NodeEditorFramework;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace GameSystem.AI.Editor
{
    public abstract class EditorAINode : Node
    {
        [MenuItem("Test/Test")]
        public static void Test()
        {
            try
            {
                string type = typeof(GameSystem.AI.Log).ToString();
                Debug.Log(type.ToString());
                Type t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.ToString() == type);
                Debug.Log(t.ToString());
            }
            catch(TypeLoadException e)
            {
                Debug.LogError(e);
            }
        }

        public abstract ConnectionKnob Parent
        {
            get;
        }

        public string dataType;
        public GenericSerializable data;

        public override string Title
        {
            get
            {
                if(dataType == null)
                    return "";
                else
                {
                    int index = dataType.LastIndexOf('.');
                    return dataType.Substring(index + 1);
                }
            }
        }

        public virtual AINode GetTreeNode()
        {
            return (AINode)data.GetObject();
        }

        public abstract IEnumerable<EditorAINode> GetChildren();

        private static object GenerateObject(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(new Type[] { });
            var o = constructor.Invoke(new object[] { }) as AINode;
            foreach(FieldInfo fi in type.GetFields())
            {
                if(fi.FieldType == typeof(string))
                {
                    fi.SetValue(o, "");
                }
            }

            return o;
        }

        public void Init(Type type)
        {
            this.dataType = type.ToString();
        }

        public override void NodeGUI()
        {
            try
            {
                if(data == null && dataType != null && dataType != "")
                    data = new GenericSerializable(dataType);
                data.DrawGUI();
            }
            catch(Exception e)
            {
                Debug.LogError(e);
                data = new GenericSerializable(dataType);
            }

            base.NodeGUI();
            GUILayout.Label(dataType);
            if(GUILayout.Button("XML"))
            {
                Debug.Log(data.ToString());
            }
        }

        void GenerateDictionary()
        {

        }
    }
}