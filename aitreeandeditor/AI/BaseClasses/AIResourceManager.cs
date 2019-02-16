using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Xml.Serialization;
using System.IO;

namespace GameSystem.AI
{
    public static class AIResourceManager
    {
        static Dictionary<string, AINode> loadedTrees = new Dictionary<string, AINode>();
        static Type[] types;
        static Type[] Types
        {
            get
            {
                if(types == null)
                    types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsSubclassOf(typeof(AINode)) && !y.IsAbstract)).ToArray();
                return types;
            }
        }

        public static AINode GetTree(string name)
        {
            if(loadedTrees.ContainsKey(name))
            {
                return loadedTrees[name];
            }
            else
            {
                string path = "AITree/" + name;
                string xml = Resources.Load<TextAsset>(path).text;
                XmlSerializer ser = new XmlSerializer(typeof(AINode), Types);
                StringReader sr = new StringReader(xml);
                AINode node = ser.Deserialize(sr) as AINode;
                if(node == null)
                    throw new NullReferenceException();
                sr.Close();
                loadedTrees.Add(name, node);
                return loadedTrees[name];
            }
        }
    }
}