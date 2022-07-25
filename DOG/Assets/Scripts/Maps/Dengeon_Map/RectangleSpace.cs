using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSpace 
{
    public Vector2Int leftDown;     // 직사각형 왼쪽 하단 좌표
    public int width;               // 너비
    public int height;              // 높이



    public RectangleSpace(Vector2Int leftDown, int width, int height)
    {
        this.leftDown = leftDown;
        this.width = width;
        this.height = height;
    }


    /// <summary>
    /// 공간의 중심 좌표를 리턴하는 함수
    /// </summary>
    /// <returns></returns>
    public Vector2Int Center()
    {
        // 길이가 짝수일 때는 정확한 중심이 없음
        return new Vector2Int(((leftDown.x * 2) + width - 1) / 2, ((leftDown.y * 2) + height - 1) / 2);
    }
}
