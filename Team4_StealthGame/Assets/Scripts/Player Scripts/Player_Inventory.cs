using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


/****************** Summary of Player Inventory ******************/

// This script should handles the Inventory Logic of the player(s)
// REMEMBER: any items in the inventory or that we create for the game should have the base class of type item
// Since the list of the inventory is of type Item(s): List<Item> inventory = new List<Item>();

/*****************************************************************/


public class Player_Inventory : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // references
    [SerializeField] Transform itemHold;
    [SerializeField] Item initialItem;
    Player_GunHandler gunHandler;
    
    // private inventory variables
    int inventoryIndex = 0;
    List<Item> inventory = new List<Item>();
    Item currentItem = null;

    void Start()
    {
        gunHandler = GetComponent<Player_GunHandler>();    

        // this code instantiates the starting Gun on the player 
        // this is only for testing purposes :D - Carlos
        if (initialItem != null)
        {
            Item newItem = Instantiate(initialItem, itemHold);
            AddItem(newItem);
        }

    }

    // OnScrollWheel is how the player iterates through its iventory
    // up or down does not matter
    void OnScrollWheel(InputValue v)
    {

        Debug.Log("Scroll Wheel function called!");

        if (inventory.Count == 0)
            return;

        if (v.Get<float>() > 0)
        {
            Debug.Log("Scroll Wheel Up");
            inventoryIndex++;
            if (inventoryIndex > inventory.Count - 1)
                inventoryIndex = 0;

            EquipItem(inventory[inventoryIndex]);
        }

        else if (v.Get<float>() < 0)
        {
            Debug.Log("Scroll Wheel Down");
            inventoryIndex--;
            if (inventoryIndex < 0)
                inventoryIndex = inventory.Count - 1;

           EquipItem(inventory[inventoryIndex]);
        }
    }

    // Once Item is added you can equip the item in the list
    // This function is called in OnScrollWheel that iterates through the items in the inventory 
    void EquipItem(Item i)
    {
        // disable current item, if there is one
        currentItem?.Unequip();
        currentItem?.gameObject.SetActive(false);

        // enable the new item
        i.gameObject.SetActive(true);
        i.transform.parent = itemHold;
        i.transform.localPosition = Vector3.zero;
        currentItem = i;

        i.Equip(this);

        // if item is gun give its gun capabilities
        if (i is Gun gun) { gunHandler.SetGun(gun); }

    }

  

    // Add item to the list
    public void AddItem(Item i)
    {
        // add new item to the list
        inventory.Add(i);

        // our index is the last one/new one
        inventoryIndex = inventory.Count - 1;

        // put item in the right place
        EquipItem(i);
    }
}
