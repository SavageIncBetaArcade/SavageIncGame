using UnityEngine;

[CreateAssetMenu(fileName = "Key", menuName = "Item/Key")]
public class KeyItem : Item
{
    public override string GetInfoDescription() { return Description; }
}