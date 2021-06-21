using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockOverride))]
public class BlockOverrideEditor : Editor
{
    int         _selected   = 0;
    string[]    _options    = new string[3] { "Item1", "Item2", "Item3" };
    private bool fold;
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (fold = EditorGUILayout.Foldout(fold, "Add Override"))
        {
            var t = target as BlockOverride;
            var shader = t.Rend.sharedMaterial.shader;
            _options = new string[shader.GetPropertyCount()];
            for (int i = 0; i < _options.Length; i++)
                _options[i] = shader.GetPropertyName(i);
            EditorGUI.BeginChangeCheck();
            this._selected = EditorGUILayout.Popup(GUIContent.none, _selected, _options);
            if (EditorGUI.EndChangeCheck())
            {
                t.CreateOverride(_selected);
            }
        }
    }
}
