using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class ExitPortal : MonoBehaviour
{
    ExitPortalKey key;

    ParticleSystem particle;
    CircleCollider2D collider;

    //Fade out 효과

    // 필드로 돌아가기
    Scene field;

    private void Awake()
    {
        key = FindObjectOfType<ExitPortalKey>();
        particle = GetComponent<ParticleSystem>();
        collider = GetComponent<CircleCollider2D>();

        particle.Stop();
        collider.enabled = false;

        key.onKeyLoot += ShowPortal;
    }

    void ShowPortal()
    {
        particle.Play();
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);  // Field Map으로 이동. 숫자 변경해야함
        }
    }
}
