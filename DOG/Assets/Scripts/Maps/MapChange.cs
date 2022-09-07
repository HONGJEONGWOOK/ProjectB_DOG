using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChange : MonoBehaviour
{
    public int sceneID = 2;
    CanvasFadeInOut fadeInOut;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        //fadeInOut.OnFadeOutEnd += SceneLoad;
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (fadeInOut == null)
        {
            fadeInOut = FindObjectOfType<CanvasFadeInOut>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("들어옴");
            fadeInOut.StartFadeOut();
            fadeInOut.OnFadeOutEnd = SceneLoad;
        }
    }

    // 다음 맵으로 넘어갈 씬의 ID
    void SceneLoad()
    {
        LoadingSceneManager.LoadScene(sceneID);
        if(sceneID == 1)
        {
            GameManager.Inst.MainPlayer.transform.localScale *= 0.5f;
        }
        else if (sceneID == 2)
        {
            GameManager.Inst.MainPlayer.transform.localScale *= 2.0f;
        }    
    }
}
