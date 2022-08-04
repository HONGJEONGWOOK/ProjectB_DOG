using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossTextController : MonoBehaviour
{
    TextMeshProUGUI textPro;
    [SerializeField] private string text;
    [SerializeField] private float typeInterval = 0.5f;

    private void Awake()
    {
        textPro = GetComponent<TextMeshProUGUI>();
        textPro.text = text;
    }

    private void OnEnable()
    {
        StartCoroutine(TextEffect());
    }

    IEnumerator TextEffect()
    {
        int letterLength = textPro.text.Length;
        int index = 0;

        while (textPro.maxVisibleCharacters < letterLength)
        {
            int letterCount = index % (letterLength);

            textPro.maxVisibleCharacters = letterCount;
            Debug.Log(textPro.maxVisibleCharacters);
            index++;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
