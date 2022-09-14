using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Boss : Monsters
{
    HP_Bar_Boss hpBar;
    BossTextController textController;
    BossRoomController roomController;
    Transform bossHitBox_1;
    Transform bossHitBox_2;
    AudioSource audioSource;

    [SerializeField] GameObject portalKey;

    private float attackRand = 0.0f;

    [Header("Meteor Stats")]
    [SerializeField] private float longAttack_Range = 4.0f;
    [Range(0, 1f)]
    [SerializeField] private float longRangeAttack_Prob = 1f;
    [SerializeField] private int fireballNum = 5;
    [Range(1f, 7f)]
    [SerializeField] private float meteorSpreadRange;

    public BossTextController TextController => textController;

    protected override void Awake()
    {
        base.Awake();
        hpBar = FindObjectOfType<HP_Bar_Boss>();
        textController = FindObjectOfType<BossTextController>();
        
        bossHitBox_1 = transform.GetChild(1);
        bossHitBox_2 = transform.GetChild(2);

        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        roomController = FindObjectOfType<BossRoomController>();
        if (roomController != null)
        {
            roomController.onBossEntry = ShowUIs;
            roomController.onReadyToFight = StartFighting;
        }

        currentSpeed = 0;
        hpBar.enabled = true;
        textController.enabled = true ;
        ChangeStatus(MonsterCurrentState.PATROL);   //처음 시작할 때 아무것도 안하기
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
        Debug.Log(this.gameObject.name);
        if (gameObject.activeSelf)
        {
            StartCoroutine(textController.TextTypingEffect());
            anim.SetFloat("AttackSelector", 0.5f);
            anim.SetInteger("CurrentStatus", 3);
            anim.SetTrigger("onAttack");
        }
    }

    void StartFighting()
    {
        status = MonsterCurrentState.TRACK;
        currentSpeed = moveSpeed;
        //audioSource.clip = SoundManager.Inst.Audios[(byte)SoundID.BossMove];
        audioSource.Play();
        audioSource.loop = true;
    }

    protected override void Idle() => ChangeStatus(MonsterCurrentState.TRACK);

    protected override void Patrol(){} // Do Nothing

    protected override void Track()
    {
        SearchPlayer();
        Move_Monster(currentSpeed);
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;
        audioSource.loop = false;
        if (attackTimer > attackCoolTime)
        {
            SpriteFlip();

            attackRand = Random.value;
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
                //audioSource.clip = SoundManager.Inst.Audios[(byte)SoundID.BossMove];
                audioSource.Play();
                audioSource.volume = 0.3f;
                audioSource.loop = true;
                return;
            }
        }
    }

    private void SpawnMeteor()
    {
        for (int i = 0; i < fireballNum; i++)
        {
            GameObject ball = EnemyBulletManager.Inst.GetPooledObject(ProjectileID.Meteor);
            Vector2 randPos = Random.insideUnitCircle * meteorSpreadRange;
            ball.transform.position = (Vector2)transform.position + randPos;
            ball.SetActive(true);
        }
    }

    protected override void SpriteFlip()
    {
        trackDirection = target.position - this.transform.position;
        var cross = Vector3.Cross(trackDirection, this.transform.up);
        if (Vector3.Dot(cross, transform.forward) < 0)
        {   // 왼쪽
            sprite.flipX = true;
            bossHitBox_1.localPosition = new Vector3(-1.35f, -0.29f);
            bossHitBox_2.localPosition = new Vector3(-0.84f, 0.25f);
        }
        else
        {   // 오른쪽
            sprite.flipX = false;
            bossHitBox_1.localPosition = new Vector3(1.35f, -0.29f);
            bossHitBox_2.localPosition = new Vector3(0.84f, 0.25f);
        }
    }

    protected override IEnumerator DisableMonster()
    {
        QuestManager.BossKillCount();

        // 죽는 사운드 재생
        audioSource.PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.BossDie].clip);

        // 시체 남기는 시간
        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length + 2.0f);

        // 죽은 뒤 폭발 애니메이션 재생
        GameObject explosion = FXManager.Inst.GetPooledFX(FxID.Explosion);
        explosion.transform.position = this.transform.position;
        explosion.SetActive(true);
        yield return new WaitForSeconds(explosion.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
        FXManager.Inst.ReturnFX(FxID.Explosion, explosion);

        DropPortalKey();

        hpBar.gameObject.SetActive(false);
        // 보스 object pool return
        MonsterManager.ReturnPooledMonster(MonsterID.BOSS, this.gameObject);
    }

    void DropPortalKey()
    {
        GameObject key = Instantiate(portalKey);
        key.transform.position = this.transform.position;
    }

    public void PlayBiteSound()
    {
        audioSource.PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.BossBite].clip, 0.5f);
    }

    public void PlayAttack1Sound()
    {
        audioSource.PlayOneShot(SoundManager.Inst.clips[(byte)SoundID.BossAttack1].clip, 0.5f);
    }

    private bool InLongRange() => (transform.position - target.position).sqrMagnitude < longAttack_Range * longAttack_Range;

#if UNITY_EDITOR
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
#endif
}