using UnityEngine;

public class AudioData : MonoBehaviour
{
    public static AudioData Instance;

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Sound sound;
        public AudioClip audioClip;
        public GameObject AudioSourcePrefab;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
