using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(YourComponent))]
public class YourComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        YourComponent yourComponent = (YourComponent)target;

        if (GUILayout.Button("Generate Chunks"))
        {
            yourComponent.Generate();
        }
    }
}
