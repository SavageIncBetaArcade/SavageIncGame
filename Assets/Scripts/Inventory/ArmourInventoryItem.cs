public class ArmourInventoryItem : InventoryItem
{
    public override void Click(Inventory inventory)
    {
        var item = new InventorySlot();
        item.Image.sprite = Item.sprite; // TODO: This line is throwing an exception
        item.InventoryItem = this;
        inventory.armourSlot.EquipItem(item);
    }
}