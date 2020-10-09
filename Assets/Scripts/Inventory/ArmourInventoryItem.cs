using UnityEngine.UI;

public class ArmourInventoryItem : InventoryItem
{
    public override void Click(Inventory inventory)
    {
        inventory.RemoveItem(Item);
        inventory.armourSlot.EquipItem(Item.sprite, this);
    }
}