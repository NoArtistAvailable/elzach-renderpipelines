using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockOverride))]
public class BlockOverrideEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Add Override"))
        {
            var t = target as BlockOverride;
            var shader = t?.Rend.sharedMaterial.shader;
            if (shader == null) return;
            var menu = new GenericMenu();
            var options = new string[shader.GetPropertyCount()];
            for (int i = 0; i < options.Length; i++)
            {
                var i1 = i;
                menu.AddItem(new GUIContent(shader.GetPropertyName(i1)), false, () => t.CreateOverride(i1));
            }
            menu.ShowAsContext();
        }
    }
}
