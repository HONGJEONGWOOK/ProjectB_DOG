using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBox : MonoBehaviour
{
    public GameObject player, puzzleObject;
    public float reach;
    bool clear;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Inst.MainPlayer.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 클리어 체크
        if (name == "PuzzleBoxRight")
        {
            clear = puzzleObject.GetComponent<Puzzle>().clear;
        }
        if (name == "PuzzleBoxLeft")
        {
            clear = puzzleObject.GetComponent<MiniPuzzle>().clear;
        }

        Vector3 playerPos = player.transform.position;  //플레이어 위치
        float distance = (transform.position - playerPos).magnitude;    //상자와 플레이어간 거리
        if (distance < reach && !clear)
        {   // 클리어 되지 않았고 설정한 거리 내로 플레이어가 상자에 접근했을 때

            // 퍼즐 활성화
            puzzleObject.SetActive(true);
            if (name == "PuzzleBoxRight")
            {
                puzzleObject.GetComponent<Puzzle>().Init();
            }
            if (name == "PuzzleBoxLeft")
            {
                puzzleObject.GetComponent<MiniPuzzle>().Init();
            }
        }
        else
        {
            puzzleObject.SetActive(false);
        }

    }
}
