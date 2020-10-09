public abstract class InventoryItem
{
    public Item Item;
    public int Quantity = 1;

    public abstract void Click(Inventory inventory);
}