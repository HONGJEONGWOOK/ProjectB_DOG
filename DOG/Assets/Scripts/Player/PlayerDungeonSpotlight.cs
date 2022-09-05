using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDungeonSpotlight : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = GameManager.Inst.MainPlayer.transform;
    }

    private void LateUpdate()
    {
        transform.position = player.position;
    }
}
