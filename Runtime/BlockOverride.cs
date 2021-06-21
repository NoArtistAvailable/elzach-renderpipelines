using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class BlockOverride : MonoBehaviour
{
    [System.Serializable]
    public class MaterialOverride
    {
        public string propertyName;

        public enum Type { Vector, Color, Texture, Bool, Float, Integer }

        public ShaderPropertyType type;

        public Vector4 vector4Value;
        public Color colorValue;
        public Texture2D textureValue;
        //public bool boolValue;
        public float floatValue;
        //public int intValue;
    }

    Renderer _rend;
    public Renderer Rend
    {
        get
        {
            if (!_rend) _rend = GetComponent<Renderer>();
            return _rend;
        }
    }

    MaterialPropertyBlock block;
    public List<MaterialOverride> overrides = new List<MaterialOverride>();

    private void OnEnable() => Set();
    private void OnValidate() => Set();
    
    private void OnDisable() => Rend.SetPropertyBlock(new MaterialPropertyBlock());




    [ContextMenu("Log Properties")]
    public void GetMaterialProperties()
    {
        var shader = _rend.sharedMaterial.shader;
        for (int i = 0; i < shader.GetPropertyCount(); i++)
        {
            Debug.Log(shader.GetPropertyName(i)
                      + " : " + shader.GetPropertyType(i)
                      + " : " + shader.GetPropertyDescription(i)
                      + " : " + shader.GetPropertyFlags(i)
                      );
        }
    }

    public void CreateOverride(int index)
    {
        var shader = Rend.sharedMaterial.shader;
        MaterialOverride prop = new MaterialOverride();
        prop.propertyName = shader.GetPropertyName(index);
        prop.type = shader.GetPropertyType(index);
        switch (prop.type)
        {
            case ShaderPropertyType.Color:
                prop.colorValue = Rend.sharedMaterial.GetColor(prop.propertyName);
                break;
            case ShaderPropertyType.Float:
                prop.floatValue = Rend.sharedMaterial.GetFloat(prop.propertyName);
                break;
            case ShaderPropertyType.Texture:
                prop.textureValue = null;
                break;
            case ShaderPropertyType.Vector:
                prop.vector4Value = Rend.sharedMaterial.GetVector(prop.propertyName);
                break;
            case ShaderPropertyType.Range:
                prop.floatValue = Rend.sharedMaterial.GetFloat(prop.propertyName);
                break;
        }
        overrides.Add(prop);
    }

    [ContextMenu("Set Block")]
    public void Set()
    {
        if (overrides == null || overrides.Count == 0) return;
        if (block == null) block = new MaterialPropertyBlock();
        Rend.GetPropertyBlock(block);
        foreach (var prop in overrides)
        {
            if (prop.propertyName.Length < 1) continue;
            switch (prop.type)
            {
                case ShaderPropertyType.Vector:
                    block.SetVector(prop.propertyName, prop.vector4Value);
                    break;
                case ShaderPropertyType.Color:
                    block.SetColor(prop.propertyName, prop.colorValue);
                    break;
                case ShaderPropertyType.Texture:
                    block.SetTexture(prop.propertyName, prop.textureValue);
                    break;
                // case SettableProperty.Type.Bool:
                //     block.SetInt(prop.propertyName, prop.boolValue ? 1 : 0);
                //     break;
                case ShaderPropertyType.Float:
                    block.SetFloat(prop.propertyName, prop.floatValue);
                    break;
                case ShaderPropertyType.Range:
                    block.SetFloat(prop.propertyName, prop.floatValue);
                    break;
                // case SettableProperty.Type.Integer:
                //     block.SetFloat(prop.propertyName, prop.intValue);
                //     break;
            }
        }
        Rend.SetPropertyBlock(block);
    }
}
