using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Game), true)]
public class GameEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Restart"))
            {
                ((Game)target).Restart();
            }
        }
    }
}
