using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinTrigger : MonoBehaviour
{
    float AttackPower;
    AudioSource audioSource;

    private void Awake()
    {
        AttackPower = transform.parent.GetComponent<Goblin>().AttackPower;
        audioSource = GetComponentInParent<AudioSource>();

        audioSource.clip = SoundManager.Inst.Audios[(byte)SoundID.GoblinHit];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IBattle target = collision.GetComponent<IBattle>();
            target.TakeDamage(AttackPower) ;
            audioSource.PlayOneShot(audioSource.clip, 0.5f);
        }
    }
}
