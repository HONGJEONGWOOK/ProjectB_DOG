using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    ParticleSystem particle;
#pragma warning disable CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.
    CircleCollider2D collider;
    AudioSource audio;
#pragma warning restore CS0108 // 멤버가 상속된 멤버를 숨깁니다. new 키워드가 없습니다.

    // 필드로 돌아가기
    Scene field;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        collider = GetComponent<CircleCollider2D>();

        particle.Stop();
        collider.enabled = false;
        audio = GetComponent<AudioSource>();
        audio.clip = SoundManager.Inst.clips[(byte)SoundID.Portal].clip;
        audio.loop = true;
    }

    public void ShowPortal()
    {
        particle.Play();
        collider.enabled = true;
        audio.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LoadingSceneManager.LoadScene(0);  // Field Map으로 이동.
        }
    }
}
