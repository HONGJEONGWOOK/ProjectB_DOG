using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFadeInOut : MonoBehaviour
{
    Canvas canvas;

    Animator anim = null;
    public System.Action OnFadeOutEnd;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canvas = transform.parent.GetComponent<Canvas>();
        canvas.OnFadeLoad += OnSceneLoad;
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoad()
    {
        StartFadeIn();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬에 있는 오브젝트가 enable이 되면 실행
        StartFadeIn();
    }

    //private void Start()
    //{
    //    StartFadeIn();
    //}

    public void StartFadeOut()
    {
        anim.SetTrigger("StageEnd");
    }

    public void StartFadeIn()
    {
        anim.SetTrigger("StageStart");
    }

    void AnimEnd()
    {
        OnFadeOutEnd?.Invoke();
    }
}
