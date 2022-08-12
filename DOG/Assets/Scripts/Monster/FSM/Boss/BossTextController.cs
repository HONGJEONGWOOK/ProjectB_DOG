using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossTextController : MonoBehaviour
{
    TextMeshProUGUI textPro;
    [SerializeField] private string text;
    [SerializeField] private float typingInterval = 0.5f;
    private string textTemp;

    private float typingTime;

    public float TypingTime { get => typingTime; }

    private void Awake()
    {
        textPro = GetComponent<TextMeshProUGUI>();
        textPro.text = null;

        typingTime = typingInterval * text.Length;
    }

    public IEnumerator TextTypingEffect()
    {
        //for (int i = 0; i < text.Length; i++)
        //{
        //    textTemp += text.Substring(0, i);
        //    textPro.text = textTemp;
        //    yield return new WaitForSeconds(typingInterval);
        //}
        foreach (char letter in text)
        {
            textPro.text += letter;
            yield return new WaitForSeconds(typingInterval);
        }
    }
}
