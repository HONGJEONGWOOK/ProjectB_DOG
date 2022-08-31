using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterPuzzleRoom : MonoBehaviour
{
    public int sceneID = 5;
    CanvasFadeInOut fadeInOut;

    public static System.Action enterPuzzleRoom;

    private void Start()
    {
        fadeInOut = FindObjectOfType<CanvasFadeInOut>();
        //fadeInOut.OnFadeOutEnd += SceneLoad;
        enterPuzzleRoom = () => { enter(); };
    }

    void enter()
    {
        fadeInOut.OnFadeOutEnd = SceneLoad;
        fadeInOut.StartFadeOut();
    }

    void SceneLoad()
    {
        LoadingSceneManager.LoadScene(sceneID);
    }
}
