using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GameSystem.AI.Editor
{
    public static class GenericEditorField
    {
        public static string DrawGenericEditor(Type type, string str, string label = "", params GUILayoutOption[] options)
        {
            if(type == typeof(bool))
            {
                return GUILayout.Toggle(bool.Parse(str), label, "Button").ToString();
            }
            if(type == typeof(string))
            {
                return EditorGUILayout.TextField((string)str, options);
            }
            else if(type == typeof(int))
            {
                return EditorGUILayout.IntField(int.Parse(str), options).ToString();
            }
            else if(type == typeof(float))
            {
                return EditorGUILayout.FloatField(float.Parse(str), options).ToString();
            }
            else if(type == typeof(Vector2))
            {
                return EditorGUILayout.Vector2Field("", ParseV2(str), options).ToString();
            }
            else if(type == typeof(Vector3))
            {
                return EditorGUILayout.Vector3Field("", ParseV3(str), options).ToString();
            }
            else if(type == typeof(Vector4))
            {
                return EditorGUILayout.Vector4Field("", ParseV4(str), options).ToString();
            }
            else
            {
                return str;
            }
        }

        private static Vector2 ParseV2(string str)
        {
            float x, y;
            string[] parts = str.Trim('(', ')').Split(',');
            x = float.Parse(parts[0]);
            y = float.Parse(parts[1]);
            return new Vector2(x, y);
        }

        private static Vector3 ParseV3(string str)
        {
            float x, y, z;
            string[] parts = str.Trim('(', ')').Split(',');
            x = float.Parse(parts[0]);
            y = float.Parse(parts[1]);
            z = float.Parse(parts[2]);
            return new Vector3(x, y, z);
        }

        private static Vector4 ParseV4(string str)
        {
            float x, y, z, w;
            string[] parts = str.Trim('(', ')').Split(',');
            x = float.Parse(parts[0]);
            y = float.Parse(parts[1]);
            z = float.Parse(parts[2]);
            w = float.Parse(parts[3]);
            return new Vector4(x, y, z, w);
        }
    }
}