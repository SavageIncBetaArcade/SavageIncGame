using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySectionHandler : MonoBehaviour
{
    public ItemInventory itemInventory;
    public AbilityInventory abilityInventory;
    public Inventory currentInventory;
    public Image inventoryTitle;
    public Button rightWeaponSlot;
    public Button rightAbilitySlot1;
    public Button rightAbilitySlot2;
    public Button rightAbilitySlot3;
    public Button rightButton;
    public Navigation rightWeaponSlotItems;
    public Navigation rightWeaponSlotAbilities;
    public Navigation rightAbilitySlot1Items;
    public Navigation rightAbilitySlot1Abilities;
    public Navigation rightAbilitySlot2Items;
    public Navigation rightAbilitySlot2Abilities;
    public Navigation rightAbilitySlot3Items;
    public Navigation rightAbilitySlot3Abilities;
    public Navigation rightButtonItems;
    public Navigation rightButtonAbilities;

    private void Start()
    {
        currentInventory = itemInventory;
    }

    public void SwitchInventories()
    {
        currentInventory.gameObject.SetActive(false);
        SwitchTo(OtherInventory());
        currentInventory.gameObject.SetActive(true);
        inventoryTitle.sprite = currentInventory.TitleImage;
        ChangeNavigations();
    }

    private void SwitchTo(Inventory newInventory)
    {
        currentInventory = newInventory;
    }
    
    private Inventory OtherInventory()
    {
        return currentInventory == itemInventory ? (Inventory) abilityInventory : itemInventory;
    }

    
    private void ChangeNavigations()
    {
        if (currentInventory == itemInventory)
        {
            SetItemNavigations();
            return;
        }
        SetAbilityNavigations();
    }

    private void SetAbilityNavigations()
    {
        rightWeaponSlot.navigation = rightWeaponSlotAbilities;
        rightAbilitySlot1.navigation = rightAbilitySlot1Abilities;
        rightAbilitySlot2.navigation = rightAbilitySlot2Abilities;
        rightAbilitySlot3.navigation = rightAbilitySlot3Abilities;
        rightButton.navigation = rightButtonAbilities;
    }

    private void SetItemNavigations()
    {
        rightWeaponSlot.navigation = rightWeaponSlotItems;
        rightAbilitySlot1.navigation = rightAbilitySlot1Items;
        rightAbilitySlot2.navigation = rightAbilitySlot2Items;
        rightAbilitySlot3.navigation = rightAbilitySlot3Items;
        rightButton.navigation = rightButtonItems;
    }
}