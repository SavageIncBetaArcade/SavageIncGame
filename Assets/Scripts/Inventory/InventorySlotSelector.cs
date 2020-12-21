using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotSelector : InfoPopupHandler, IPointerClickHandler
{
    public InventorySectionHandler inventorySectionHandler;
    private Inventory Inventory => inventorySectionHandler.currentInventory;
    public int position;
    protected override Item Item => Inventory.getItemAt(position);

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Inventory.LeftClickItem(position);
                break;
            case PointerEventData.InputButton.Right:
                Inventory.RightClickItem(position);
                break;
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != gameObject) return;
        if(Input.GetKeyDown("joystick button 0"))
            Inventory.LeftClickItem(position);
        else if(Input.GetKeyDown("joystick button 2"))
            Inventory.RightClickItem(position);
    }
}