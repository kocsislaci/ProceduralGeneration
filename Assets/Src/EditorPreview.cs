using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazePreviewer))]
public class EditorPreview : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Preivew!"))
        {
            MazePreviewer pw = (MazePreviewer)target;

            pw.GeneratePreview();
        }

        if (GUILayout.Button("Destroy Preivew!"))
        {

            MazePreviewer pw = (MazePreviewer)target;

            pw.DestroyPreivew();
        }
    }
}
