using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Inventory inventory;
    public Item armourItem;
    public Item armourItem2;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            inventory.AddItem(armourItem);
        else if (Input.GetKeyDown(KeyCode.R))
            inventory.AddItem(armourItem2);
    }
}