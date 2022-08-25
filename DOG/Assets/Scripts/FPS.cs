using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    float dTime = 0f;
    TextMeshProUGUI fpsText;

    WaitForSeconds waitSeconds;

    float fps = 0f;
    private void Awake()
    {
        fpsText = GetComponent<TextMeshProUGUI>();
        waitSeconds = new WaitForSeconds(1.0f);

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        StartCoroutine(ShowFPS());
    }
    private void Update()
    {
        dTime += (Time.unscaledDeltaTime - dTime) * 0.1f;
    }

    IEnumerator ShowFPS()
    {
        while (true)
        {
            fps = 1 / dTime;
            fpsText.text = $"FPS : {fps:F2} ms";
            yield return waitSeconds;
        }
    }
}
