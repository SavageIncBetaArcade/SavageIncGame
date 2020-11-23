using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public string Quote;
    public Sprite Sprite;
    public List<ScriptableModifier> modifiers;
    public abstract string GetInfoDescription();
}