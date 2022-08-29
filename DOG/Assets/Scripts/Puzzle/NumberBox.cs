using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberBox : MonoBehaviour
{
    public int index = 0;
    float x = 0;
    float y = 0;

    private Action<int, int> swapFunc = null;

    public void Init(float i, float j, int index, Sprite sprite, Action<int, int> swapFunc)
    {
        this.index = index;
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        UpdatePos(i, j);
        this.swapFunc = swapFunc;
    }
    public void UpdatePos(float i, float j)
    {
        x = i;
        y = j;
        StartCoroutine(Move());
    }
    IEnumerator Move()                  //타일 움직임
    {
        float elapsedTime = 0;
        float duration = 0.1f;
        Vector2 start = this.gameObject.transform.localPosition;
        Vector2 end = new Vector2(x, y);

        while(elapsedTime<duration)
        {
            this.gameObject.transform.localPosition = Vector2.Lerp(start, end, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.gameObject.transform.localPosition = end;
    }

    public bool IsEmpty()
    {
        return index == 16; 
    }
    public bool IsEmpty_mini()
    {
        return index == 9;
    }

    void OnMouseDown()       
    {
        if (Input.GetMouseButtonDown(0) && swapFunc != null)
            swapFunc((int)x, (int)y);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Player"))
        {
            Debug.Log(collision.name + " Destoryed");
            Destroy(collision.gameObject);
        }
    }*/
}
