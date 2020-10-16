using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Inventory inventory;
    public int position;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                inventory.LeftClickItem(position);
                break;
            case PointerEventData.InputButton.Right:
                inventory.RightClickItem(position);
                break;
        }
    }
}