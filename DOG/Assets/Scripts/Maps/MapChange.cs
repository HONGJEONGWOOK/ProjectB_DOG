using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapChange : MonoBehaviour
{
    public int sceneID = 2;
    CanvasFadeInOut fadeInOut;

    private void Start()
    {
        fadeInOut = FindObjectOfType<CanvasFadeInOut>();
        //fadeInOut.OnFadeOutEnd += SceneLoad;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("들어옴");
            fadeInOut.OnFadeOutEnd = SceneLoad;
            fadeInOut.StartFadeOut();
        }
    }

    // 다음 맵으로 넘어갈 씬의 ID
    void SceneLoad()
    {
        LoadingSceneManager.LoadScene(sceneID);
    }
}
