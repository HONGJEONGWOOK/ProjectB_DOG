using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inven : MonoBehaviour
{
    ItemInventory_UI invenUI;

    private void Awake()
    {
        invenUI = FindObjectOfType<ItemInventory_UI>();
    }

    void Start()
    {
        Inventory inven = new Inventory();
        invenUI.InitializeInven(inven);

        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.ManaPotion);
        //inven.AddItem(ItemID.GoblinsPPP);
        //inven.AddItem(ItemID.Arrows);

        inven.AddItem(ItemID.HPPotion);

        inven.AddItem(ItemID.HPPotion, 3);
        inven.AddItem(ItemID.HPPotion, 3);
        inven.AddItem(ItemID.HPPotion, 3);
        //inven.MoveItem(0, 2);

        inven.TestInventory();

    }
}
