using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Inst => instance;

    AudioSource source;

    [SerializeField] SoundID sounds;

    // Environment
    public AudioClip[] clips;
    //BGM = 0,
    //playerFootStep 1,
    //playerGetHit 2,
    //swingWeapon 3,
    //click 4,
    //windowOpen 5,
    //pointerOnSlot 6,
    //weaponChange 7,
    //potionUse 8,
    //QuestComplete 9,
    //GoblinHit 10,
    //BossHit, 11
    //BossBite, 12
    //BossAttack1, 13
    //BossDie, 14
    //MeteorFly,15
    //MeteorExplosion, 16
    //ShootArrow 17

    //ClipNumbers

    Dictionary<byte, AudioClip> audios = new();
    public Dictionary<byte, AudioClip> Audios => audios;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Initialize()
    {
        source = GetComponent<AudioSource>();

        for(int i = 0; i < (int)SoundID.ClipNumbers; i++)
        {
            audios.Add((byte)i, clips[i]);
        }
    }

    public void PlaySound(SoundID clipID, float volume , bool oneShot = false)
    {
        source.clip = clips[(int)clipID];
        source.volume = volume;
        
        if (oneShot)
        {
            source.PlayOneShot(clips[(int)clipID]);
        }
        else
        {
            source.Play();
        }
    }
}
