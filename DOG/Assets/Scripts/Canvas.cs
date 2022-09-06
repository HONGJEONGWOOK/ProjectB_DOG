using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Canvas : MonoBehaviour
{
    private static Canvas instance;
    public static Canvas Instance { get { return instance; } }

    CanvasGroup group;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoad;
            group = GetComponent<CanvasGroup>();
            DontDestroyOnLoad(this.gameObject);     // 씬이 변경되더라도 게임 오브젝트가 사라지기 않게 해주는 함수
        }
        else
        {
            // 씬의 Gamemanager가 여러번 생성됐다.
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 4)
        {// 로딩 씬
            group.alpha = 0f;
        }
        else
        {
            group.alpha = 1f;
        }
    }
}
