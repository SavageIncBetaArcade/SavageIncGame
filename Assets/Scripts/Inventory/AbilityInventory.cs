using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInventory : Inventory
{
    public EquipSlot[] leftHand = new EquipSlot[4]; 
    public EquipSlot[] rightHand = new EquipSlot[4];
    public ScriptableUseableAbility[] StartingAbilities;
    public Text warningText;
    public override string Title => "Abilities";
    
    void Start()
    {
        foreach (var ability in StartingAbilities)
        {
            AddItem(ability);
        }
    }

    public override void EquipLeftHand(InventoryItem abilityToEquip)
    {
        if (!leftHand.Any(abilitySlot => EquipAbilityInSlot(abilityToEquip as AbilityInventoryItem, abilitySlot)))
            StartCoroutine(ShowWarningText());
    }

    public override void EquipRightHand(InventoryItem abilityToEquip)
    {
        if (!rightHand.Any(abilitySlot => EquipAbilityInSlot(abilityToEquip as AbilityInventoryItem, abilitySlot)))
            StartCoroutine(ShowWarningText());
    }

    public override void EquipCenter(InventoryItem itemToEquip)
    {
        throw new System.NotImplementedException();
    }

    private bool EquipAbilityInSlot(AbilityInventoryItem abilityToEquip, EquipSlot abilitySlot)
    {
        if (abilitySlot.equippedSlot.InventoryItem != null) return false;
        RemoveItem(abilityToEquip.Item);
        abilitySlot.EquipItem(abilityToEquip);
        return true;
    }
    
    private IEnumerator ShowWarningText()
    {
        warningText.enabled = true;
        yield return new WaitForSeconds(3);
        warningText.enabled = false;
    }
}