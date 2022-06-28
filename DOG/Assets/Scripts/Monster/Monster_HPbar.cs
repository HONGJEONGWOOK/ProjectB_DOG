using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_HPbar : MonoBehaviour
{
    private RectTransform rect = null;
    private Vector2 pos = Vector2.zero;
    private Camera cam = null;

    private Monsters monster = null;

    [Header("Monster UI Tweaks")]
    public Vector3 offset = Vector3.zero;

    private void Awake()
    {
        monster = FindObjectOfType<Monsters>();
        rect = GetComponent<RectTransform>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        FollowMonster();
    }

    void FollowMonster()
    {
        if (!Monsters.isDead)
        {
            pos = cam.WorldToScreenPoint(monster.transform.position + offset);
            if ((Vector2)rect.position != pos)
            {
                rect.position = pos;
            }
        }
    }

    void HP_Gauge()
    {
        //HP에 따라 체력 변하게
    }
}
