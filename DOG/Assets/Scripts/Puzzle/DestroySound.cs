using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySound : MonoBehaviour
{
    AudioSource audio;
    MiniPuzzle mini;
    Puzzle rightpz;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        mini = FindObjectOfType<MiniPuzzle>();

        rightpz = FindObjectOfType<Puzzle>();
        rightpz.OngateOpen += PlayGateOpenSound;
        mini.OnRockDestroy += PlaySound;
    }

    private void PlaySound()
    {
        audio.clip = SoundManager.Inst.clips[(byte)SoundID.Rock].clip;
        audio.Play();
    }
    private void PlayGateOpenSound()
    {
        audio.clip = SoundManager.Inst.clips[(byte)SoundID.Gate].clip;
        audio.Play();
    }
}
