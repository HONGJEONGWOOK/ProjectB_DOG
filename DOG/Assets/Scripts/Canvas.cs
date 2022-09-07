using System;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Canvas : MonoBehaviour
{
    private static Canvas instance;

    CanvasGroup group;

    public System.Action OnFadeLoad;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            group = GetComponent<CanvasGroup>();
            SceneManager.sceneLoaded += OnSceneLoad;
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
        OnFadeLoad?.Invoke();
    }
}
