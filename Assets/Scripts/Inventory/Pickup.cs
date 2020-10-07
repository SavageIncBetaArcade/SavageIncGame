using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Inventory inventory;
    public Item item;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(item);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            inventory.RemoveItem(item);
        }
    }
}