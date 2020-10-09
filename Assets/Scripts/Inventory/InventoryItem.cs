public abstract class InventoryItem
{
    public Item Item;
    public int Quantity = 1;

    public abstract void LeftClick(Inventory inventory);
    public abstract void RightClick(Inventory inventory);
}