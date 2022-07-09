using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSpace 
{
    public Vector2Int leftDown;     // ���簢�� ���� �ϴ� ��ǥ
    public int width;               // �ʺ�
    public int height;              // ����

    public RectangleSpace(Vector2Int leftDown, int width, int height)
    {
        this.leftDown = leftDown;
        this.width = width;
        this.height = height;
    }


    /// <summary>
    /// ������ �߽� ��ǥ�� �����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    public Vector2Int Center()
    {
        return new Vector2Int(((leftDown.x * 2) + width - 1) / 2, ((leftDown.y * 2) + height - 1) / 2);
    }
}
