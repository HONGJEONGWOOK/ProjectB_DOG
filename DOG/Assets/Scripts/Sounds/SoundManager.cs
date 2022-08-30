using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Inst => instance;

    AudioSource source;

    public AudioClipDatas[] clips;

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
    }

    public void PlaySound(SoundID clipID, bool oneShot = false)
    {
        if (oneShot)
        {
            source.PlayOneShot(clips[(int)clipID].clip);
        }
        else
        {
            source.clip = clips[(int)clipID].clip;
            source.Play();
        }
        source.volume = clips[(int)clipID].volume;
    }
}
