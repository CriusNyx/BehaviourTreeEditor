using System.IO;
using UnityEngine;
using UnityEditor;
using GameSystem.AI.Editor;

public class AIFileEditor : EditorWindow
{
    [MenuItem("Tools/AIEditor")]
    public static void GetAIEditor()
    {
        EditorWindow.GetWindow<AIFileEditor>();
    }

    string searchPattern = "";
    Vector2 scrollPosition = Vector2.zero;

    public void OnGUI()
    {
        string[] files = Directory.GetFiles(Path.Combine(Application.dataPath, "AI/Resources/AITree"), "*" + searchPattern + "*.xml");

        searchPattern = EditorGUILayout.TextField(searchPattern);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        {
            foreach(var file in files)
            {
                if(GUILayout.Button(Path.GetFileNameWithoutExtension(file)))
                {
                    AINodeCanvas.LoadFile(file);
                }
            }
        }
        GUILayout.EndScrollView();
    }
}