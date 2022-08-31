using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CircleCollider2D))]
public class ExitPortal : MonoBehaviour
{
    ParticleSystem particle;
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
    CircleCollider2D collider;
#pragma warning restore CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.

    //Fade out 효과

    // 필드로 돌아가기
    Scene field;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        collider = GetComponent<CircleCollider2D>();

        particle.Stop();
        collider.enabled = false;
    }

    public void ShowPortal()
    {
        particle.Play();
        collider.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene(0);  // Field Map으로 이동.
        }
    }
}
