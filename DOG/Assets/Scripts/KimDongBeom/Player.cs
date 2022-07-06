using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager manager;

    //움직임 관련
    float h;
    float v;
    public float speed;
    Rigidbody2D rigid;

    //캐릭터의 방향을 알아내기 위해
    Vector2 direction;

    //캐릭터의 방향에 녹색 선에 있는 게임 오브젝트를 불러오기 위해
    GameObject scanObject;

    private void Awake()
    {
        //사용하기 위해 리지드 바디 초기화
        rigid = GetComponent<Rigidbody2D>();
    }

    
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //이동, 2D라서 Vector2 위 아래로만
        rigid.velocity = new Vector2(h, v).normalized * speed;

        //DrawRay 선을 보려고 유니티 내에서 녹색으로 그어보는 기능
        //DrawRay(플레이어 포지션, 방향 * 선 거리, 색깔)
        Debug.DrawRay(rigid.position, direction * 1.0f, new Color(0, 1, 0));

        //RaycastHit2D/Physics2D 하나의 클래스인듯? 감지를 위한 rayHit 변수이름
        //3D면 RaycastHit Physics 라고 사용하면 됌
        //(Ray 원점, Ray 방향, 충돌 감지할 RaycastHit, Ray 거리(길이))
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, direction, 1.0f, LayerMask.GetMask("Object"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }

    void Update()
    {
        h = manager.isAction ? 0 : Input.GetAxisRaw("Horizontal");
        v = manager.isAction ? 0 : Input.GetAxisRaw("Vertical");

        if (v == 1)
        {
            direction = Vector3.up;
        }
        else if (v == -1)
        {
            direction = Vector3.down;
        }
        else if (h == 1)
        {
            direction = Vector3.right;
        }
        else if (h == -1)
        {
            direction = Vector3.left;
        }

        //rayHit에 들어온 레이어 오브젝트가 널이 아니면 감지하여 대화걸기
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            manager.AskAction(scanObject);
        }
    }
}
