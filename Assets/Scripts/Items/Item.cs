using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public string Quote;
    public Sprite Sprite;
    public List<ScriptableModifier> modifiers;
    public abstract string GetInfoDescription();
    public string AssetPath;

    [ContextMenu("Get Asset Path")]
    private void GetAssetPath()
    {
        AssetPath = AssetDatabase.GetAssetPath(this).Replace("Assets/","");
    }
}

//custom editor