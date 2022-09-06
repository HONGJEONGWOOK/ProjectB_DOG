using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Inst.MainPlayer.transform.position = transform.position;
    }
}
