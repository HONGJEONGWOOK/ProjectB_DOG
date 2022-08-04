using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossTextController : MonoBehaviour
{
    TextMeshProUGUI textPro;
    [SerializeField] private string text;
    [SerializeField] private float typeInterval = 0.2f;

    private void Awake()
    {
        textPro = GetComponent<TextMeshProUGUI>();
        textPro.text = text;
    }

    IEnumerator TextEffect()
    {
        int letterLength = textPro.text.Length;
        int index = 0;
        Debug.Log(textPro.maxVisibleCharacters);
        while (textPro.maxVisibleCharacters < letterLength)
        {
            int letterCount = index % (letterLength);

            textPro.maxVisibleCharacters = letterCount;
            Debug.Log(textPro.maxVisibleCharacters);
            index++;
            yield return new WaitForSeconds(typeInterval);
        }
    }
}
