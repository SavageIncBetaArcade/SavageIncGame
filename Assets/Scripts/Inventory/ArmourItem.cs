using UnityEngine;

[CreateAssetMenu(fileName = "Armour", menuName = "Item/Armour")]
public class ArmourItem : Item
{
    public override void Click()
    {
        var armourSlot = FindObjectOfType<EquipSlot>();
        var item = new InventoryItem();
        Debug.Log(sprite.name);
        item.Image.sprite = sprite; // TODO: This line is throwing an exception
        item.Item = this;
        armourSlot.EquipItem(item);
    }
}