using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Inst => instance;

    AudioSource source;
    AudioSource backgroundSource;

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
        backgroundSource = transform.GetChild(0).GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode arg1)
    {
        switch (scene.buildIndex)
        {
            case 0: // House
                backgroundSource.clip = clips[(int)SoundID.BGM_Minigame].clip;
                backgroundSource.volume = clips[(int)SoundID.BGM_Minigame].volume;
                backgroundSource.Play();
                backgroundSource.loop = true;
                break;
            case 1: // Field and Village
                backgroundSource.clip = clips[(int)SoundID.BGM_Field].clip;
                backgroundSource.volume = clips[(int)SoundID.BGM_Field].volume;
                backgroundSource.Play();
                backgroundSource.loop = true;
                break;
            case 2: // dungeon
                backgroundSource.clip = clips[(int)SoundID.BGM_Dungeon].clip;
                backgroundSource.volume = clips[(int)SoundID.BGM_Dungeon].volume;
                backgroundSource.Play();
                backgroundSource.loop = true;
                break;
            case 3: // boss
                backgroundSource.clip = clips[(int)SoundID.BGM_Boss].clip;
                backgroundSource.volume = clips[(int)SoundID.BGM_Boss].volume;
                backgroundSource.Play();
                backgroundSource.loop = true;
                break;
            case 4: // minigame
                backgroundSource.clip = clips[(int)SoundID.BGM_Minigame].clip;
                backgroundSource.volume = clips[(int)SoundID.BGM_Minigame].volume;
                backgroundSource.Play();
                backgroundSource.loop = true;
                break;
            case 5: // loading
                backgroundSource.Stop();
                break;
        }
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

    public void ChangeBGM(SoundID soundID)
    {
        backgroundSource.clip = clips[(int)soundID].clip;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }
}
