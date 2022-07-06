using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager manager;

    //������ ����
    float h;
    float v;
    public float speed;
    Rigidbody2D rigid;

    //ĳ������ ������ �˾Ƴ��� ����
    Vector2 direction;

    //ĳ������ ���⿡ ��� ���� �ִ� ���� ������Ʈ�� �ҷ����� ����
    GameObject scanObject;

    private void Awake()
    {
        //����ϱ� ���� ������ �ٵ� �ʱ�ȭ
        rigid = GetComponent<Rigidbody2D>();
    }

    
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        //�̵�, 2D�� Vector2 �� �Ʒ��θ�
        rigid.velocity = new Vector2(h, v).normalized * speed;

        //DrawRay ���� ������ ����Ƽ ������ ������� �׾�� ���
        //DrawRay(�÷��̾� ������, ���� * �� �Ÿ�, ����)
        Debug.DrawRay(rigid.position, direction * 1.0f, new Color(0, 1, 0));

        //RaycastHit2D/Physics2D �ϳ��� Ŭ�����ε�? ������ ���� rayHit �����̸�
        //3D�� RaycastHit Physics ��� ����ϸ� ��
        //(Ray ����, Ray ����, �浹 ������ RaycastHit, Ray �Ÿ�(����))
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

        //rayHit�� ���� ���̾� ������Ʈ�� ���� �ƴϸ� �����Ͽ� ��ȭ�ɱ�
        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            manager.AskAction(scanObject);
        }
    }
}
