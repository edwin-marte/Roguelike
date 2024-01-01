using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public enum Sound
    {
        PlayerShot,
        EnemyHit,
        CollectSoul,
        BackgroundMusic,
        LevelUp
    }

    private Dictionary<Sound, float> soundCooldowns = new Dictionary<Sound, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void PlaySound(Sound sound, float cooldownTime = 0.2f)
    {
        if (Instance == null)
        {
            Debug.LogError("AudioManager instance not found.");
            return;
        }

        var audioData = Instance.GetAudioData(sound);

        if (!audioData.Item2.GetComponent<AudioSource>().loop)
        {
            if (!Instance.CanPlaySound(sound))
            {
                return;
            }
        }

        var soundGameObject = Instantiate(audioData.Item2, Vector2.zero, Quaternion.identity);

        var audioSource = soundGameObject.GetComponent<AudioSource>();
        audioSource.clip = audioData.Item1;
        audioSource.Play();

        if (!audioData.Item2.GetComponent<AudioSource>().loop)
        {
            Instance.soundCooldowns[sound] = Time.time + cooldownTime;
            float pitchAdjustedLength = audioSource.clip.length / audioSource.pitch;
            // Destroy the sound GameObject after the sound has finished playing
            Instance.StartCoroutine(Instance.DestroySoundAfterPlay(pitchAdjustedLength, soundGameObject, sound));
        }
    }

    private IEnumerator DestroySoundAfterPlay(float delay, GameObject soundGameObject, Sound sound)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + delay;

        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }

        // Remove the sound from the dictionary and destroy the GameObject
        if (Instance.soundCooldowns.ContainsKey(sound))
        {
            Instance.soundCooldowns.Remove(sound);
        }
        Destroy(soundGameObject);
    }

    private bool CanPlaySound(Sound sound)
    {
        if (!soundCooldowns.ContainsKey(sound))
        {
            soundCooldowns.Add(sound, 0f);
            return true;
        }

        return Time.time >= soundCooldowns[sound];
    }

    private (AudioClip, GameObject) GetAudioData(Sound sound)
    {
        foreach (AudioData.SoundAudioClip soundAudioClip in AudioData.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return (soundAudioClip.audioClip, soundAudioClip.AudioSourcePrefab);
            }
        }
        Debug.LogError("Sound " + sound + " not found");
        return (null, null);
    }
}
