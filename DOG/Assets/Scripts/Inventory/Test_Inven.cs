using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Inven : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Inventory inven = new Inventory();

        inven.AddItem(ItemID.HPPotion);
        inven.AddItem(ItemID.ManaPotion);
        inven.AddItem(ItemID.GoblinsPPP);
        inven.AddItem(ItemID.Arrows);

        inven.AddItem(ItemID.HPPotion);
        inven.RemoveItem(0);


        inven.TestInventory();
        inven.InitializeInven();
    }
}
