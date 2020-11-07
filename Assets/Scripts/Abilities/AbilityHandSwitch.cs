using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHandSwitch : MonoBehaviour
{
    public enum HandType
    {
        LEFT,
        RIGHT   
    }

    public Image CurrentAbilityImage;
    public Image AbilityImage1;
    public Image AbilityImage2;
    public Image AbilityImage3;

    public ScriptableUseableAbility AbilitySlot1;
    public ScriptableUseableAbility AbilitySlot2;
    public ScriptableUseableAbility AbilitySlot3;
    public ScriptableUseableAbility AbilitySlot4;

    public InventorySectionHandler inventoryHandler;
    public HandType HandTye;
    public UseableAbility PlayerAbility;

    private EquipSlot[] handSlots;

    private Sprite baseSprite;
    private string cycleHandButtonName;

    void Awake()
    {
        if (!inventoryHandler)
        {
            Debug.LogError("Inventory Handler not sent in AbilityHandSwitch");
            return;
        }

        baseSprite = CurrentAbilityImage.sprite;

        switch (HandTye)
        {
            case HandType.LEFT:
                handSlots = inventoryHandler.abilityInventory.leftHand;
                cycleHandButtonName = "CycleLeftHand";
                break;
            case HandType.RIGHT:
                handSlots = inventoryHandler.abilityInventory.rightHand;
                cycleHandButtonName = "CycleRightHand";
                break;
        }

        setupHand();
    }

    private void Update()
    {
        if (Input.GetButtonDown(cycleHandButtonName))
        {
            shiftHand();
        }
    }

    private void setupHand()
    {
        foreach (var equipSlot in handSlots)
        {
            equipSlot.ItemChangedEvent += EquipSlot_ItemChangedEvent;
        }
    }

    void OnDestroy()
    {
        foreach (var equipSlot in handSlots)
        {
            equipSlot.ItemChangedEvent -= EquipSlot_ItemChangedEvent;
        }
    }


    private void EquipSlot_ItemChangedEvent(InventoryItem inventoryItem, InventoryItem oldItem, EquipSlot slot)
    {
        int slotIndex = Array.IndexOf(handSlots, slot);
        if (slotIndex < 0) return;

        SetAbilitySlot(slotIndex, inventoryItem);
    }

    private void SetAbilitySlot(int index, InventoryItem inventoryItem)
    {
        ScriptableUseableAbility ability = inventoryItem?.Item as ScriptableUseableAbility;
        Sprite slotSprite = ability ? ability.Sprite : baseSprite;

        switch (index)
        {
            case 0:
                AbilitySlot1 = ability;
                CurrentAbilityImage.sprite = slotSprite;

                //also set the players ability
                PlayerAbility?.SetAbility(ability);
                break;
            case 1:
                AbilitySlot2 = ability;
                AbilityImage1.sprite = slotSprite;
                break;
            case 2:
                AbilitySlot3 = ability;
                AbilityImage2.sprite = slotSprite;
                break;
            case 3:
                AbilitySlot4 = ability;
                AbilityImage3.sprite = slotSprite;
                break;
        }
    }

    private void shiftHand()
    {
        EquipSlot[] temp = new EquipSlot[handSlots.Length];
        for (int i = 0; i < handSlots.Length; i++)
        {
            int index = (i + 1) % temp.Length;
            temp[index] = handSlots[i];
            SetAbilitySlot(index, handSlots[i].equippedSlot.InventoryItem);
        }

        handSlots = temp;
    }
}
