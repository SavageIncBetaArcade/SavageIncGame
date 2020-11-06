using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public string Quote;
    public Sprite Sprite;
    public abstract string GetInfoDescription();
}