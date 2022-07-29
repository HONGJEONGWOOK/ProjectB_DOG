using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    Transform shadow;
    SpriteRenderer shadowRenderer;
    GameObject ball;
    Rigidbody2D ballRigid;
    ParticleSystem ballParticle;
    SpriteRenderer ballSprite;

    Animator explosion;

    [SerializeField] private float speed = 3.0f;
    [Range(0f,1f)]
    [SerializeField] private float shadowTweaker = 0.5f;
    [SerializeField] private Vector2 initialPosition = new Vector2(-3, 7);
    float alpha = 0f;
    float endTimer = 0.0f;

    private void Awake()
    {
        shadow = transform.GetChild(0);
        shadowRenderer = shadow.GetComponent<SpriteRenderer>();
        ball = transform.GetChild(1).gameObject;
        ballRigid = ball.GetComponent<Rigidbody2D>();
        ballParticle = ball.GetComponentInChildren<ParticleSystem>();
        ballSprite = ball.GetComponent<SpriteRenderer>();

        explosion = transform.GetChild(2).GetComponent<Animator>();
    }

    private void OnEnable()
    {
        InitializeMeteor();
    }

    private void FixedUpdate()
    {
        ballRigid.position = Vector2.MoveTowards(ballRigid.position, shadow.position,
            speed * Time.fixedDeltaTime);

        if (IsCollided())
        {
            ballParticle.Stop();
            explosion.SetTrigger("isCollided");
            ballSprite.color = Color.clear;
            shadowRenderer.color = Color.clear;
        }
    }

    private void Update()
    {
        if (ballSprite.color != Color.clear)
        {
            alpha = Mathf.Lerp(alpha, 1.0f, Time.deltaTime * shadowTweaker);
            shadowRenderer.color = new Color(0, 0, 0, alpha);
        }

        if (IsCollided())
        {
            endTimer += Time.deltaTime;
            if (endTimer > explosion.GetCurrentAnimatorStateInfo(0).length)
            {
                EnemyBulletManager.Inst.ReturnPooledEnemy(
                    EnemyBulletManager.PooledObjects[EnemyBulletManager.Inst.MeteorID], this.gameObject);
                endTimer = 0f;
            }
        }
    }

    bool IsCollided() => ((Vector2)shadow.position - ballRigid.position ).sqrMagnitude < 0.1f;

    void InitializeMeteor()
    {
        ballParticle.Play();
        ballSprite.color = Color.white;
        ball.transform.localPosition = initialPosition;
    }
}
