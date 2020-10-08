public abstract class InventoryItem
{
    public Item Item;
    public int Quanity = 1;

    public abstract void Click(Inventory inventory);
}