using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Inventory inventory;
    public Item armourItem;
    public Item armourItem2;
    public Item weaponItem;
    public Item weaponItem2;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            inventory.AddItem(armourItem);
        else if (Input.GetKeyDown(KeyCode.R))
            inventory.AddItem(armourItem2);
        else if(Input.GetKeyDown(KeyCode.T))
            inventory.AddItem(weaponItem);
        else if(Input.GetKeyDown(KeyCode.Y))
            inventory.AddItem(weaponItem2);
    }
}