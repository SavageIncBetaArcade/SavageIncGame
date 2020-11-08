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

    public Image[] AbilityImages = new Image[4];
    public Image[] CoolDownImages = new Image[4];

    public InventorySectionHandler inventoryHandler;
    public HandType HandTye;
    public UseableAbility PlayerAbility;

    private EquipSlot[] handSlots;

    private ScriptableUseableAbility[] AbilitySlots = new ScriptableUseableAbility[4];

    private Sprite baseSprite;
    private string cycleHandButtonName;

    void Awake()
    {
        if (!inventoryHandler)
        {
            Debug.LogError("Inventory Handler not sent in AbilityHandSwitch");
            return;
        }

        baseSprite = AbilityImages[0]?.sprite;

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

        updateCooldowns();
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

        AbilitySlots[index] = ability;
        AbilityImages[index].sprite = slotSprite;

        if(index == 0)
            PlayerAbility?.SetAbility(ability); //also set the players ability
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

    private void updateCooldowns()
    {
        for (int i = 0; i < CoolDownImages.Length; i++)
        {
            if(!CoolDownImages[i])
                continue;
            
            //get the ability for the cooldown
            ScriptableUseableAbility ability = handSlots[i].equippedSlot.InventoryItem?.Item as ScriptableUseableAbility;

            if (!ability)
            {
                CoolDownImages[i].enabled = false;
                continue;
            }

            //check the last use time for the ability in the players useable ability hand
            float lastUseTime = 0.0f;
            if (PlayerAbility.LastUseDictionary.ContainsKey(ability))
                lastUseTime = PlayerAbility.LastUseDictionary[ability];

            float cooldownTime = Mathf.Min((Time.time - lastUseTime) / ability.Cooldown, 1);

            if (cooldownTime >= 1.0f && CoolDownImages[i].IsActive())
                CoolDownImages[i].enabled = false;
            else if (cooldownTime < 1.0f && !CoolDownImages[i].IsActive())
                CoolDownImages[i].enabled = true;

            CoolDownImages[i].fillAmount = cooldownTime;
        }
    }
}
