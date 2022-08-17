using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Item Datas", order = 2)]
public class ItemData : ScriptableObject
{
    [SerializeField] public uint id = 0;
    [SerializeField] public string itemName = "새로운 아이템";
    [SerializeField] public uint count = 0;
    [SerializeField] public uint maxCount = 5;

    [SerializeField] public GameObject prefab;
    [SerializeField] public Sprite icon;
}
