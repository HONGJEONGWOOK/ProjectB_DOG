using UnityEngine;

[System.Serializable]
public class AudioClipDatas
{
    public string clipName = "New Clip";
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 0.5f;
}
