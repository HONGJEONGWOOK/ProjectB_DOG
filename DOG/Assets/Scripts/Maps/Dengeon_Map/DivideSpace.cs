using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DivideSpace 전체공간. totalSpace 나누고 리스트에 저장
/// </summary>
public class DivideSpace : MonoBehaviour
{
    public int totalWidth;  // 전체공간의 너비
    public int totalHeight; // 전체공간의 높이
    [SerializeField]
    private int minWidth;   // 공간이 가질 수 있는 최소 너비
    [SerializeField]
    private int minHeight;  // 공간이 가질 수 있는 최소 높이

    public RectangleSpace totalSpace;   // 전체공간

    public List<RectangleSpace> spaceList;  // 나누어진 공간들을 저장하는 리스트


    /// <summary>
    /// 공간의 너비 또는 높이가 최소치 * 2 이상이면 공간을 나눈다
    /// </summary>
    /// <param name="space"></param>
    public void DivideRoom(RectangleSpace space)
    {
        if(space.height >= minHeight *2 && space.width >= minWidth * 2)     // 가로 또는 세로로 자른다.
        {
            if(Random.Range(0,2) < 1)
            {
                RectangleSpace[] spaces = DivideHorizontal(space);

                DivideRoom(spaces[0]);
                DivideRoom(spaces[1]);
            }
            else
            {
                RectangleSpace[] spaces = Dividevertical(space);

                DivideRoom(spaces[0]);
                DivideRoom(spaces[1]);
            }
        }
        else if(space.height < minHeight * 2 && space.width >= minWidth * 2)    // 세로로 자른다.
        {
            RectangleSpace[] spaces = Dividevertical(space);

            DivideRoom(spaces[0]);
            DivideRoom(spaces[1]);
        }
        else if(space.height >= minHeight * 2 && space.width < minWidth * 2)    // 가로로 자른다.
        {
            RectangleSpace[] spaces = DivideHorizontal(space);

            DivideRoom(spaces[0]);
            DivideRoom(spaces[1]);
        }
        else    // 자를 수 없는 상태이기 떄문에 자르른 것을 멈추고 해당 공간을 리스트에 저장한다.
        {
            spaceList.Add(space);
        }
    }


    /// <summary>
    /// 공간을 가로로 자르는 함수
    /// </summary>
    /// <param name="space"></param>
    /// <returns></returns>
    private RectangleSpace[] DivideHorizontal(RectangleSpace space)
    {
        int newSpace1Height = minHeight + Random.Range(0, space.height - minHeight * 2 + 1);    // 새로운 공간 좌표를 정할때 기존 공간의 좌표에서 최소 높이에 랜덤한 높이를 더해준다
        RectangleSpace newSpace1 = new RectangleSpace(space.leftDown, space.width, newSpace1Height);

        int newSpace2Height = space.height - newSpace1Height;
        Vector2Int newSpace2LeftDown = new Vector2Int(space.leftDown.x, space.leftDown.y + newSpace1Height);
        RectangleSpace newSpace2 = new RectangleSpace(newSpace2LeftDown, space.width, newSpace2Height);

        RectangleSpace[] spaces = new RectangleSpace[2];
        spaces[0] = newSpace1;
        spaces[1] = newSpace2;
        return spaces;
    }


    /// <summary>
    /// 공간을 세로로 자른다.
    /// </summary>
    /// <param name="space"></param>
    /// <returns></returns>
    private RectangleSpace[] Dividevertical(RectangleSpace space)
    {
        int newSpace1Width = minWidth + Random.Range(0, space.width - minWidth * 2 + 1);    // 새로운 공간 좌표를 정할때 기존 공간의 좌표에서 최소 너비에 랜덤한 너비를 더해준다
        RectangleSpace newSpace1 = new RectangleSpace(space.leftDown, newSpace1Width, space.height);

        int newSpace2Width = space.width - newSpace1Width;
        Vector2Int newSpace2LeftDown = new Vector2Int(space.leftDown.x + newSpace1Width, space.leftDown.y );
        RectangleSpace newSpace2 = new RectangleSpace(newSpace2LeftDown, newSpace2Width,space.height );

        RectangleSpace[] spaces = new RectangleSpace[2];
        spaces[0] = newSpace1;
        spaces[1] = newSpace2;
        return spaces;
    }
}
