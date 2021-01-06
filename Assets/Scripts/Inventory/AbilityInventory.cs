using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AbilityInventory : Inventory
{
    public EquipSlot[] leftHand = new EquipSlot[4]; 
    public EquipSlot[] rightHand = new EquipSlot[4];
    public ScriptableUseableAbility[] StartingAbilities;
    public Text warningText;
    public override Sprite TitleImage => Resources.Load<Sprite>("Inventory/ability");

    protected override void Awake()
    {
        base.Awake();

        foreach (var ability in StartingAbilities)
        {
            AddItem(ability, true);
        }
    }

    public override void EquipLeftHand(InventoryItem abilityToEquip, bool swap)
    {
        if (!leftHand.Any(abilitySlot => EquipAbilityInSlot(abilityToEquip as AbilityInventoryItem, abilitySlot)))
            StartCoroutine(ShowWarningText());
    }

    public override void EquipRightHand(InventoryItem abilityToEquip, bool swap)
    {
        if (!rightHand.Any(abilitySlot => EquipAbilityInSlot(abilityToEquip as AbilityInventoryItem, abilitySlot)))
            StartCoroutine(ShowWarningText());
    }

    public override void EquipCenter(InventoryItem itemToEquip, bool swap)
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

    #region IDataPersistance

    public override void  Save(DataContext context)
    {
        base.Save(context);

        //save left hand
        Dictionary<int, string> leftHandPaths = new Dictionary<int, string>();
        for (int i = 0; i < leftHand.Length; i++)
        {
            EquipSlot slot = leftHand[i];
            if (slot.equippedSlot.InventoryItem != null && slot.equippedSlot.InventoryItem.Item)
            {
                leftHandPaths.Add(i, slot.equippedSlot.InventoryItem.Item
                    .AssetPath.Replace("Resources/", "").Replace(".asset", ""));
            }
        }

        //save right hand
        Dictionary<int, string> rightHandPaths = new Dictionary<int, string>();
        for (int i = 0; i < rightHand.Length; i++)
        {
            EquipSlot slot = rightHand[i];
            if (slot.equippedSlot.InventoryItem != null && slot.equippedSlot.InventoryItem.Item)
            {
                rightHandPaths.Add(i, slot.equippedSlot.InventoryItem.Item
                    .AssetPath.Replace("Resources/", "").Replace(".asset", ""));
            }
        }


        context.SaveData(uuid.ID, "leftHand", leftHandPaths);
        context.SaveData(uuid.ID, "rightHand", rightHandPaths);
    }

    public override void Load(DataContext context,bool destroyUnloaded = false)
    {
        base.Load(context, destroyUnloaded);

        //remove current slots
        for (int i = 0; i < leftHand.Length; i++)
        {
            leftHand[i].RemoveItem();
        }
        for (int i = 0; i < rightHand.Length; i++)
        {
            rightHand[i].RemoveItem();
        }

        //save left hand
        Dictionary<int, string> leftHandPaths = context.GetValue<Dictionary<int, string>>(uuid.ID, "leftHand");
        for (int i = 0; i < leftHand.Length; i++)
        {
            if(leftHandPaths != null && leftHandPaths.ContainsKey(i))
            {
                var ability = Resources.Load<Item>(leftHandPaths[i]);
                //To improve this we can equip the ability in the slot it was saved in
                if (ability)
                {
                    var invItem = AddItem(ability, false);
                    invItem.LeftClick(this,character);
                };
            }
        }

        //save right hand
        Dictionary<int, string> rightHandPaths = context.GetValue<Dictionary<int, string>>(uuid.ID, "rightHand");
        for (int i = 0; i < rightHand.Length; i++)
        {
            if (rightHandPaths != null && rightHandPaths.ContainsKey(i))
            {
                var ability = Resources.Load<Item>(rightHandPaths[i]);
                //To improve this we can equip the ability in the slot it was saved in
                if (ability)
                {
                    var invItem = AddItem(ability, false);
                    invItem.RightClick(this, character);
                };
            }
        }
    }

    #endregion
}