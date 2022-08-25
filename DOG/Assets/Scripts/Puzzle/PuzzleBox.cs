using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : MonoBehaviour
{
    public GameObject player, puzzleObject;
    public float reach;
    // Start is called before the first frame update
    void Awake()
    {
        puzzleObject.SetActive(true);
        if (name == "PuzzleBoxRight")
        {
            puzzleObject.GetComponent<Puzzle>().Init();
        }
        if(name == "PuzzleBoxLeft")
        {
            puzzleObject.GetComponent<MiniPuzzle>().Init();
        }
        puzzleObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.transform.position;  //플레이어 위치
        float distance = (transform.position - playerPos).magnitude;    //상자와 플레이어간 거리
        if (distance < reach)
        {
            puzzleObject.SetActive(true);
        }
        else
        {
            puzzleObject.SetActive(false);
        }

    }
}
