using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Linq;

namespace GameSystem.AI.Editor
{
    [Serializable]
    public class GenericSerializable
    {
        [SerializeField]
        string xml;
        [NonSerialized]
        XElement xel;
        [SerializeField]
        string dataType;
        [NonSerialized]
        List<Action> actions = new List<Action>();

        public GenericSerializable()
        {

        }

        public GenericSerializable(string dataType)
        {
            this.dataType = dataType;
        }

        public void DrawGUI()
        {
            if(xel == null)
                DynamicInit();

            foreach(var action in actions)
            {
                action();
            }
        }

        public object GetObject()
        {
            Type t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.ToString() == dataType);

            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsSubclassOf(t) || t.IsSubclassOf(y))).ToArray();
                //Assembly.GetAssembly(t).GetTypes().Where(x => x.IsSubclassOf(t) || t.IsSubclassOf(x)).ToArray();
            XmlSerializer ser = new XmlSerializer(t, types);
            StringReader sr = new StringReader(xml);
            object output = ser.Deserialize(sr);
            sr.Close();
            return output;
        }

        public void SetObject(object o, Type[] types)
        {
            this.dataType = o.GetType().ToString();
            XmlSerializer ser = new XmlSerializer(o.GetType(), types);
            StringWriter sw = new StringWriter();
            ser.Serialize(sw, o);
            xml = sw.ToString();
            sw.Close();
        }

        private void DynamicInit()
        {
            if(xml == null)
                NewInit();
            OldInit();
        }

        private void NewInit()
        {
            //create object
            Type t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.ToString() == dataType);
            ConstructorInfo ci = t.GetConstructor(new Type[] { });
            object o = ci.Invoke(new object[] { });
            foreach(FieldInfo fi in t.GetFields())
            {
                if(fi.FieldType == typeof(string))
                {
                    fi.SetValue(o, "");
                }
            }

            //serialize object to xml
            StringWriter sw = new StringWriter();
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.IsSubclassOf(t) || y.IsSubclassOf(y))).ToArray();
                //Assembly.GetAssembly(t).GetTypes().Where(x => x.IsSubclassOf(t) || t.IsSubclassOf(x)).ToArray();
            XmlSerializer ser = new XmlSerializer(t, types);
            ser.Serialize(sw, o);
            xml = sw.ToString();
            sw.Close();
        }

        private void OldInit()
        {
            Type t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).FirstOrDefault(x => x.ToString() == dataType);

            //create xml doc
            xel = XElement.Parse(xml);

            //ser up events
            foreach(FieldInfo fi in t.GetFields())
            {
                XElement local = xel.Nodes().FirstOrDefault(x => ((XElement)x).Name == fi.Name) as XElement;
                if(fi.FieldType == typeof(string))
                {
                    actions.Add(() =>
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(fi.Name);
                        string value = local.Value;
                        local.SetValue(GenericEditorField.DrawGenericEditor(fi.FieldType, local.Value));
                        if(local.Value != value)
                            xml = xel.ToString();
                        GUILayout.EndHorizontal();
                    });
                }
                if(fi.FieldType == typeof(bool))
                {
                    actions.Add(() =>
                    {
                        string value = local.Value;
                        local.SetValue(GenericEditorField.DrawGenericEditor(fi.FieldType, local.Value, fi.Name));
                        if(local.Value != value)
                            xml = xel.ToString();
                    }
                    );
                }
            }
        }


        public override string ToString()
        {
            return xml;
        }
    }
}