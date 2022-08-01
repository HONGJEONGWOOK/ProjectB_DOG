using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolingData
{
    public string objectName = "";
    public int objectID = 0;
    public GameObject prefab = null;
    public int poolSize = 0;
}
