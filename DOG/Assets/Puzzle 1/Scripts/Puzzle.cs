using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public NumberBox boxPrefab;

    public NumberBox[,] boxes = new NumberBox[4, 4];

    public Sprite[] sprites;
    int[][] correct = new int[4][];

    bool check;
    void Start()
    {
       // Init();
    }
    private void Update()
    {
        if(check)
            SuccessCheck();

        if(Input.GetKeyDown(KeyCode.Home) && check)
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
    void SuccessCheck()
    {

        int i, j = 0;
        for(i = 0; i <= 3; i++)
        {
            for(j= 0; j <= 3; j++)
            {
                //Debug.Log("i : " + i + " j : " + j + " index : " + boxes[i, j].index) //확인용
                if(correct[i][j] != boxes[i, j].index)
                {
                    break;
                }
            }
            if (j < 4)
                break;
        }
        if (i == 4 && j == 4)
        {
            Debug.Log("Success");   //문
        }
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
