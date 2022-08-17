using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 무기 데이터 매니저
public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas;

    public ItemData this[uint i]
    {
        get => itemDatas[i];
    }

    public ItemData this[ItemID id]
    {
        get => itemDatas[(uint)id];
    }
}
