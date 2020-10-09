using UnityEngine.UI;

public class ArmourInventoryItem : InventoryItem
{
    public override void Click(Inventory inventory)
    {
        inventory.armourSlot.EquipItem(Item.sprite, this);
    }
}