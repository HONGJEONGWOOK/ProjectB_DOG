using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boss : Monsters
{
    HP_Bar_Boss hpBar;
    BossTextController textController;
    BossRoomController roomController;
    [SerializeField] GameObject portalKey;

    private float attackRand = 0.0f;

    [Header("Meteor Stats")]
    [SerializeField] private float longAttack_Range = 4.0f;
    [Range(0, 1f)]
    [SerializeField] private float longRangeAttack_Prob = 1f;
    [SerializeField] private int fireballNum = 5;
    [Range(1f, 7f)]
    [SerializeField] private float meteorSpreadRange;

    protected override void Awake()
    {
        base.Awake();
        hpBar = FindObjectOfType<HP_Bar_Boss>();
        textController = FindObjectOfType<BossTextController>();
        roomController = FindObjectOfType<BossRoomController>();
    }

    private void OnEnable()
    {
        roomController.onBossEntry += ShowUIs;
        roomController.onReadyToFight += StartFighting;

        currentSpeed = 0;
        hpBar.enabled = false;
        textController.enabled = false;
        status = MonsterCurrentState.IDLE;
    }

    private void OnDisable()
    {
        hpBar.gameObject.SetActive(false);
        textController.gameObject.SetActive(false);
    }

    void ShowUIs()
    {
        hpBar.gameObject.SetActive(true);
        textController.gameObject.SetActive(true);
    }

    void StartFighting()
    {
        status = MonsterCurrentState.TRACK;
        currentSpeed = moveSpeed;
    }

    protected override void Idle()  {} // Do Nothing

    protected override void Track()
    {
        SearchPlayer();
        Move_Monster(currentSpeed);
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;
        
        if (attackTimer > attackCoolTime)
        {
            SpriteFlip();

            attackRand = Random.value;
            Debug.Log(attackRand);
            if (attackRand > (1 - longRangeAttack_Prob))
            { // 원거리 공격 메테오
                SpawnMeteor();
            }
            anim.SetFloat("AttackSelector", attackRand);
            anim.SetTrigger("onAttack");
            attackTimer = 0.0f;
        }
        else if ((attackTimer < attackCoolTime) && !InLongRange())
        {
            detectTimer += Time.deltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(MonsterCurrentState.TRACK);
                detectTimer = 0f;
                return;
            }
        }
    }

    private void SpawnMeteor()
    {
        for (int i = 0; i < fireballNum; i++)
        {
            GameObject ball = 
                EnemyBulletManager.Inst.GetPooledObject(EnemyBulletManager.PooledObjects[EnemyBulletManager.Inst.MeteorID]);
            Vector2 randPos = Random.insideUnitCircle * meteorSpreadRange;
            ball.transform.position = (Vector2)transform.position + randPos;
            ball.SetActive(true);
        }
    }

    protected override IEnumerator DisableMonster()
    {
        // 시체 남기는 시간
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length + 2.0f);

        // 죽은 뒤 폭발 애니메이션 재생
        GameObject explosion = 
            FXManager.Inst.GetPooledFX(FXManager.PooledFX[FXManager.Inst.ExplosionID]);
        explosion.transform.position = this.transform.position;
        yield return new WaitForSeconds(2.0f);
        FXManager.Inst.ReturnFX(FXManager.PooledFX[FXManager.Inst.ExplosionID], explosion);

        DropPortalKey();

        // 보스 object pool return
        MonsterManager.Inst.ReturnPooledMonster(MonsterManager.PooledMonster[MonsterManager.Inst.BossID], 
                                                this.gameObject);
    }

    void DropPortalKey()
    {
        GameObject key = Instantiate(portalKey);
        key.transform.position = this.transform.position;
    }


    private bool InLongRange() => (transform.position - target.position).sqrMagnitude < longAttack_Range * longAttack_Range;


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.forward, longAttack_Range);
        if (attackRand > 0.75f)
        {
            Handles.color = Color.red;
        }
    }
}