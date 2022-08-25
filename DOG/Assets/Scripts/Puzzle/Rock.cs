using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public bool check;
    public int dir;
    public float freezePos_x,freezePos_y;
    void Start()
    {
        check = false;
        dir = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(check)
        {
 //           transform.GetComponent<Rigidbody2D>().simulated = true;

            if (dir == 0 || dir == 2)
            {
                Vector3 pos = transform.position;
                pos.y = freezePos_y;
                transform.position = pos;
            }
            else if (dir == 1 || dir == 3)
            {
                Vector3 pos = transform.position;
                pos.x = freezePos_x;
                transform.position = pos;
            }
        }
        else
        {
//           transform.GetComponent<Rigidbody2D>().simulated = false;
        }
    }
}
