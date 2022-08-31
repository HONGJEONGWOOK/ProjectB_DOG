using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentMapIndicator : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI text;

    float showSpeed = 1f;
    float showTime = 2.0f;
    float showTimer = 0f;

    bool isShown = false;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.alpha = 0f;
    }

    private void Start()
    {
        StartCoroutine(ShowMapName());
    }

    public IEnumerator ShowMapName()
    {
        while (!isShown)
        {
            showTimer += Time.deltaTime;
            text.alpha = showTimer * showSpeed;
            if (showTimer > showTime)
            {
                isShown = true;
                StartCoroutine(HideMapName());
            }
            yield return null;
        }
    }

    IEnumerator HideMapName()
    {
        while (isShown)
        {
            showTimer -= Time.deltaTime;
            text.alpha = showTimer * showSpeed;
            if (showTimer < 0f)
            { 
                isShown = false;
                showTimer = 0f;
            }
            yield return null;
        }
    }

}
