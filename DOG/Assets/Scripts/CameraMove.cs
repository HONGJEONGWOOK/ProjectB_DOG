using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Player_Hero player;

    private void Start()
    {
        player = GameManager.Inst.MainPlayer;
    }
    private void LateUpdate()
    {
        Vector3 camPos = player.transform.position;
        camPos.z = -10f;
        transform.position = camPos;
    }
}
