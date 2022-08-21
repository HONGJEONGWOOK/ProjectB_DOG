using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static int nextScene;
    [SerializeField] Slider LoadingBar;

    [SerializeField] float loadSpeed = 3.0f;

    MonsterManager monManager;

    private void Awake()
    {
        monManager = FindObjectOfType<MonsterManager>();
    }

    // Start is called before the first frame update
    void Start()
    {// 로딩씬 재생
        StartCoroutine(LoadScene());
    }

    /// <summary>
    /// 다음 씬으로 넘어가기 전 로딩씬 불러오기
    /// </summary>
    /// <param name="nextSceneIndex">로딩씬 다음으로 로드할 씬</param>
    public static void LoadScene(int nextSceneIndex)
    {
        MonsterManager.ReturnAllMonsters();

        nextScene = nextSceneIndex;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation AsyncOp = SceneManager.LoadSceneAsync(nextScene);
        AsyncOp.allowSceneActivation = false;
        float timer = 0f;
        while (!AsyncOp.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (AsyncOp.progress < 0.8f)
            {
                LoadingBar.value = Mathf.Lerp(LoadingBar.value, AsyncOp.progress, loadSpeed * Time.deltaTime);
            }
            else
            {
                LoadingBar.value = Mathf.Lerp(LoadingBar.value, 1f, loadSpeed * Time.deltaTime);
                if (LoadingBar.value > 0.95f)
                {
                    AsyncOp.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
