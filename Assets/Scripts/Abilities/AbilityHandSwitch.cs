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
    public HandType Hand;
    public UseableAbility PlayerAbility;
    public ScriptableMeleeAbility BaseAbility;

    private EquipSlot[] handSlots;
    private ScriptableUseableAbility[] AbilitySlots = new ScriptableUseableAbility[4];
    private Sprite baseSprite;
    private string cycleHandButtonName;


    void Awake()
    {
        if (!inventoryHandler)
        {
            Debug.LogError("Inventory Handler not sent in AbilityHandSwitch");
            enabled = false;
            return;
        }
        if (!PlayerAbility)
        {
            Debug.LogError("Player useable ability not sent in AbilityHandSwitch");
            enabled = false;
            return;
        }

        baseSprite = AbilityImages[0]?.sprite;

        switch (Hand)
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

        //setup weapon equip events
        switch (Hand)
        {
            case HandType.LEFT:
                inventoryHandler.itemInventory.leftWeaponSlot.ItemChangedEvent += EquipSlot_ItemChangedEvent;
                break;
            case HandType.RIGHT:
                inventoryHandler.itemInventory.rightWeaponSlot.ItemChangedEvent += EquipSlot_ItemChangedEvent;
                break;
        }
    }

    void OnDestroy()
    {
        foreach (var equipSlot in handSlots)
        {
            equipSlot.ItemChangedEvent -= EquipSlot_ItemChangedEvent;
        }

        //setup weapon equip events
        switch (Hand)
        {
            case HandType.LEFT:
                inventoryHandler.itemInventory.leftWeaponSlot.ItemChangedEvent -= EquipSlot_ItemChangedEvent;
                break;
            case HandType.RIGHT:
                inventoryHandler.itemInventory.rightWeaponSlot.ItemChangedEvent -= EquipSlot_ItemChangedEvent;
                break;
        }
    }

    private void EquipSlot_ItemChangedEvent(InventoryItem inventoryItem, InventoryItem oldItem, EquipSlot slot)
    {
        int slotIndex = Array.IndexOf(handSlots, slot);
        if (slotIndex < 0)
        {
            //weapon was uneqiped, remove it from the players hand
            if(oldItem?.Item is WeaponItem)
            {
                SetAbilitySlot(-1, null);
            }
            else if(handSlots[0].equippedSlot.InventoryItem?.Item == null)
                SetAbilitySlot(0, inventoryItem);
            return;
        }
        SetAbilitySlot(slotIndex, inventoryItem);
    }

    private void SetAbilitySlot(int index, InventoryItem inventoryItem)
    {
        ScriptableUseableAbility ability = inventoryItem?.Item as ScriptableUseableAbility;
        Sprite slotSprite = ability ? ability.Sprite : baseSprite;

        if (index >= 0)
        {
            AbilitySlots[index] = ability;
            AbilityImages[index].sprite = slotSprite;
        }

        if (index == 0)
        {
            // if the new ability is null then default to the players weapon (Base Attack)
            if (!ability && BaseAbility)
            {
                WeaponItem weapon = inventoryItem?.Item as WeaponItem;
                //if weapon is null try and get the weapon from the equiped slot instead
                if(!weapon)
                {
                    switch (Hand)
                    {
                        case HandType.LEFT:
                            weapon = inventoryHandler.itemInventory.leftWeaponSlot.equippedSlot.InventoryItem?.Item as WeaponItem;
                            break;
                        case HandType.RIGHT:
                            weapon = inventoryHandler.itemInventory.rightWeaponSlot.equippedSlot.InventoryItem?.Item as WeaponItem;
                            break;
                        default:
                            break;
                    }
                }

                if(weapon)
                    PlayerAbility?.SetWeapon(weapon, BaseAbility);
                else
                    PlayerAbility?.SetAbility(null);
            }
            else
            {
                PlayerAbility?.SetAbility(ability); //also set the players ability
            }
        }
        else
        {
            //index was -1 meaning there was no ability to be set so remove the current player ability
            PlayerAbility?.SetAbility(null);
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
            float lastUseTime = Time.time;
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
