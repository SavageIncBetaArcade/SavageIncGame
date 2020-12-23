﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySectionHandler : MonoBehaviour
{
    public ItemInventory itemInventory;
    public AbilityInventory abilityInventory;
    public Inventory currentInventory;
    public Image inventoryTitle;

    private void Start()
    {
        currentInventory = itemInventory;
        abilityInventory.gameObject.SetActive(false);
    }

    public void SwitchInventories()
    {
        currentInventory.gameObject.SetActive(false);
        SwitchTo(OtherInventory());
        currentInventory.gameObject.SetActive(true);
        inventoryTitle.sprite = currentInventory.TitleImage;
    }

    private void SwitchTo(Inventory newInventory)
    {
        currentInventory = newInventory;
    }
    
    private Inventory OtherInventory()
    {
        return currentInventory == itemInventory ? (Inventory) abilityInventory : itemInventory;
    }
}