using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_Trigger : MonoBehaviour
{
    Rock rock;
    public int dir; //0번 9시, 1번 12시, 2번 3시, 3번째 6시

    private void Start()
    {
        rock = transform.parent.GetComponent<Rock>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && rock.check == false)
        {
            rock.check = true;
            rock.dir = dir;
            rock.freezePos_x = rock.transform.position.x;
            rock.freezePos_y = rock.transform.position.y;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rock.check = false;
            rock.dir = -1;
        }
    }
}
