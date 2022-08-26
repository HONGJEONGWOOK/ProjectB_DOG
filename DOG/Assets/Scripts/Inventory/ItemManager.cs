using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Inst => instance;

    private Dictionary<uint, Stack<GameObject>> pooledItems = new();
    public Dictionary<uint, Stack<GameObject>> PooledItems => pooledItems;

    [SerializeField] private ItemData[] poolingObjects;
    public ItemData[] PoolingObjects => poolingObjects;

    Stack<GameObject>[] items = new Stack<GameObject>[4];

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        for (uint i = 0; i < poolingObjects.Length; i++)
        {
            items[i] = new Stack<GameObject>();
            pooledItems.Add(i, items[i]);
        }

        for (uint i = 0; i < poolingObjects.Length; i++)
        {
            for (int j = 0; j < poolingObjects[i].poolingSize; j++)
            {
                GameObject obj = Instantiate(PoolingObjects[i].prefab, this.transform);
                obj.GetComponent<Items>().data = poolingObjects[i];
                pooledItems[i].Push(obj);
                obj.SetActive(false);
            }
        }
    }

    public static GameObject GetPooledItem(Stack<GameObject> poolingStack)
    {
        if (poolingStack.Count > 0)
        {
            GameObject obj = poolingStack.Pop();
            return obj;
        }

        return null;
    }

    public static void ReturnItem(Stack<GameObject> returningStack, GameObject uselessItem)
    {
        returningStack.Push(uselessItem);
        uselessItem.SetActive(false);
        uselessItem.transform.position = Vector2.zero;
    }
}
