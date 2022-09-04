using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefab;

    public NumberBox[,] boxes = new NumberBox[4, 4];

    //사운드
    public System.Action OngateOpen;

    public GameObject mapAssets;
    float defaultPlayerSpeed;
    Player_Hero player;
    public GameObject pzGate;

    public Sprite[] sprites;
    int[][] correct = new int[4][];

    bool check;
    public bool clear;

    PlayerInputActions playerActions;   // PlayerInputActions 객체 선언

    void Start()
    {
        // PlayerInputActioin 생성자, 필요한 콜백함수 넣기 및 활성화
        playerActions = new();
        playerActions.UI.PuzzleCheat.performed += OnPuzzleCheat;    // 필요한 Input
        playerActions.UI.Enable();
        // ===================================================

        player = GameObject.FindWithTag("Player").GetComponent<Player_Hero>();  // 플레이어 참조
        defaultPlayerSpeed = player.moveSpeed;  // 플레이어 기본 이동속도 저장
        // Init();
    }
    private void Update()
    {
        if (check && !clear)
        {
            QuizModeActive(true);
            SuccessCheck();
        }
    }

    private void OnPuzzleCheat(InputAction.CallbackContext context)
    {
        if (playerActions.UI.PuzzleCheat.ReadValue<float>() > 0)
        {
            Debug.Log("Cheat Button Pressed");

            if (check)
            {
                for (int i = 0; i <= 3; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        correct[i][j] = boxes[i, j].index;
                    }
                }
            }
        }
    }

    void QuizModeActive(bool active)
    {   // 온전히 퀴즈만 풀게 하는 상태로 돌입하는 함수

        if (active)
        {
            player.moveSpeed = 0;   // 플레이어 기본 이동속도 0으로
            mapAssets.SetActive(false);   // 맵 요소들 비활성화
        }
        else
        {
            player.moveSpeed = defaultPlayerSpeed; // 플레이어 기본 이동속도 복구
            mapAssets.SetActive(true);   // 맵 요소들 활성화
        }
    }

    void SuccessCheck()
    {

        int i, j = 0;
        for (i = 0; i <= 3; i++)
        {
            for (j = 0; j <= 3; j++)
            {
                //Debug.Log("i : " + i + " j : " + j + " index : " + boxes[i, j].index) //확인용
                if (correct[i][j] != boxes[i, j].index)
                {
                    break;
                }
            }
            if (j < 4)
                break;
        }
        if (i == 4 && j == 4)
        {
            SuccessProcess();
        }
    }

    void SuccessProcess()
    {
        // 퍼즐 성공 부분
        Debug.Log("Puzzle Success");   //문
        clear = true;   // 클리어 체크
        QuizModeActive(false);  // 퍼즐 모드 끄기

        pzGate = GameObject.Find("Gate");
        Destroy(pzGate);
        Debug.Log("Gate Destroy");
        OngateOpen?.Invoke();

        //퍼즐 완료 시 다음 퀘스트로 
        QuestManager.Instance.NextQuest();

        gameObject.SetActive(false);    // 퍼즐 없애기
    }

    void ClickToSwap(int x, int y)
    {
        int dx = getDx(x, y);
        int dy = getDy(x, y);
        Swap(x, y, dx, dy);

    }

    // Update is called once per frame
    public void Init()
    {
        if(check)
        {
            return;
        }
        int n = 0;
        for (int y = 3; y >= 0; y--)
            for(int x = 0; x < 4; x++)
            {
                NumberBox box = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity);
                box.Init(x, y, n + 1, sprites[n], ClickToSwap);
                box.transform.parent = transform;
                boxes[x, y] = box;
                n++;
            }
        for (int i = 0; i < 999; i++)
            Shuffle();
        check = true;

        //정답배열 만들기
        for(int i = 0; i < 4; i++)
        {
            int value = 13 + i;
            correct[i] = new int[4];
            for (int j = 0; j < 4; j++)
            {
                correct[i][j] = value;
                value -= 4;
            }
        }
    }
    void Swap(int x, int y,int dx, int dy)
    {

        var from = boxes[x, y];
        var target = boxes[x + dx, y + dy];

        //두 상자를 바꾸는
        boxes[x, y] = target;
        boxes[x + dx, y + dy] = from;

        //2 박스 위치 업데이트
        from.UpdatePos(x + dx, y + dy);
        target.UpdatePos(x , y);
    }
    
    int getDx(int x, int y)
    {
        //오른쪽이 비어있는
        if(x<3 && boxes[x + 1, y].IsEmpty())
            return 1;
        //왼쪽이 비어있는
        if (x > 0 && boxes[x + -1, y].IsEmpty())
            return -1;

        return 0;

        }

    int getDy(int x, int y)
    {
        //위가 비어있는
        if (y < 3 && boxes[x, y + 1].IsEmpty())
            return 1;
        //아래가 비어있는
        if (y > 0 && boxes[x , y - 1].IsEmpty())
            return -1;

        return 0;

    }

    void Shuffle()
    {
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                if(boxes[i, j].IsEmpty())
                {
                    Vector2 pos = getValidMove(i, j);
                    Swap(i, j, (int)pos.x, (int)pos.y);
                }
            }
        }
    }

    private Vector2 lastMove;
    Vector2 getValidMove(int x, int y)
    {
        Vector2 pos = new Vector2();

        do
        {
            int n = Random.Range(0, 4);
            if (n == 0)
                pos = Vector2.left;
            else if (n == 1)
                pos = Vector2.right;
            else if (n == 2)
                pos = Vector2.up;
            else
                pos = Vector2.down;
        } while (!(isValidRange(x + (int)pos.x) && isValidRange(y + (int)pos.y)) || isRepeatMove(pos));

        lastMove = pos;
        return pos;
    }

    bool isValidRange(int n)
    {
        return n >= 0 && n <= 3;
    }

    bool isRepeatMove(Vector2 pos)
    {
        return pos * -1 == lastMove;
    }
}
