using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(BlockOverride.MaterialOverride))]
public class MaterialOverrideDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 1;
    }

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //var serializedMember = GetFieldOrPropertyValue //(BlockRenderer.SettableProperty) property.serializedObject.targetObject as BlockRenderer.SettableProperty;
        
        // FieldInfo fieldInfo = property.serializedObject.targetObject.GetType().GetField(property.displayName, BindingFlags.Public);
        // if (fieldInfo == null) return;
        // BlockRenderer.SettableProperty serializedMember = (BlockRenderer.SettableProperty) fieldInfo.GetValue(property.serializedObject.targetObject);
        
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var currentRect = new Rect(position);
        currentRect.height = EditorGUIUtility.singleLineHeight;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        
        // EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockRenderer.SettableProperty.propertyName)), GUIContent.none);
        //currentRect.y += EditorGUIUtility.singleLineHeight;
        
        var typeProperty = property.FindPropertyRelative(nameof(BlockOverride.MaterialOverride.type));
        // EditorGUI.PropertyField(currentRect, typeProperty, GUIContent.none);
        // currentRect.y += EditorGUIUtility.singleLineHeight;
        if (true)
        {
            var propertyType =
                (ShaderPropertyType) typeProperty.intValue; // as BlockRenderer.SettableProperty.Type;
            // var propertyValue = property.FindPropertyRelative(nameof(BlockRenderer.SettableProperty.value));
            switch (propertyType)
            {
                case ShaderPropertyType.Vector:
                    var guiContent = GUIContent.none;
                    var vector4field =
                        property.FindPropertyRelative(nameof(BlockOverride.MaterialOverride.vector4Value));
                    vector4field.vector4Value = EditorGUI.Vector4Field(currentRect, guiContent, vector4field.vector4Value);
                    break;
                case ShaderPropertyType.Color:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockOverride.MaterialOverride.colorValue)), GUIContent.none);
                    break;
                // case BlockRenderer.SettableProperty.Type.Bool:
                //     EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockRenderer.SettableProperty.boolValue)), GUIContent.none);
                //     break;
                case ShaderPropertyType.Float:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockOverride.MaterialOverride.floatValue)), GUIContent.none);
                    break;
                // case BlockRenderer.SettableProperty.Type.Integer:
                //     EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockRenderer.SettableProperty.intValue)), GUIContent.none);
                //     break;
                case ShaderPropertyType.Texture:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockOverride.MaterialOverride.textureValue)), GUIContent.none);
                    break;
                case ShaderPropertyType.Range:
                    EditorGUI.PropertyField(currentRect, property.FindPropertyRelative(nameof(BlockOverride.MaterialOverride.floatValue)), GUIContent.none);
                    break;
            }
        }
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
