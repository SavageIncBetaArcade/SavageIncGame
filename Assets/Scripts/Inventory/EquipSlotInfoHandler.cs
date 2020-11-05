public class EquipSlotInfoHandler : InfoPopupHandler
{
    protected override Item Item => GetComponent<EquipSlot>().equippedSlot.InventoryItem.Item;
}