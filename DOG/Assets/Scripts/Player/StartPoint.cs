using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    private Player_Hero player;
    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player_Hero>();
        }

        player.transform.position = transform.position;
    }


}
