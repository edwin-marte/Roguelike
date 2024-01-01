using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public float spawnRate;
    public int desiredAmountToSpawn;
    public float startTime;
    [HideInInspector] public int waveIndex;
}

[System.Serializable]
public class Wave
{
    public EnemyType[] enemyTypes; // List of enemy types for this wave
    public float duration;
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    public UIManager uiManager;
    public InGameManager inGameManager;

    public EnemySpawner enemySpawner;
    private bool waveRunning = false;

    private int waveIndex = 0;
    private float currentDuration = 0f;
    private float currentTime = 0f;
    private Coroutine countdown;

    public Wave[] waves; // List of waves, each containing enemy types for that wave

    private void Awake()
    {
        Instance = this;
    }

    public bool WaveRunning()
    {
        return waveRunning;
    }

    private IEnumerator Countdown()
    {
        currentTime = waves[0].duration;
        waveRunning = true;

        while (currentTime >= 0f)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;

            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            uiManager.currentTimeText.text = string.Format("{0}:{1:00}", minutes, seconds);

            if (currentTime <= 0f)
            {
                waveRunning = false;
                inGameManager.CleanScreen();
                StopCoroutine(countdown);
                inGameManager.PauseGame();

                UIManager.Instance.upgradePanel.SetActive(true);
                inGameManager.GenerateUpgradeOptions();
            }
        }
    }

    public void StartNextWave()
    {
        if (waveIndex < waves.Length)
        {
            Wave currentWave = waves[waveIndex];

            foreach (var enemyType in currentWave.enemyTypes)
            {
                enemySpawner.SetSpawnRateAndAmount(enemyType.enemyPrefab, enemyType.spawnRate, enemyType.startTime, enemyType.desiredAmountToSpawn, waveIndex);
            }

            waveIndex++;
            uiManager.currentWaveText.text = "Wave " + waveIndex;
            currentDuration = waves[waveIndex - 1].duration; ;
            uiManager.currentTimeText.text = currentDuration.ToString();
            countdown = StartCoroutine(Countdown());
            inGameManager.ResumeGame();
        }
    }

    public void ResetWaves()
    {
        waveIndex = 0;
        currentDuration = waves[0].duration;
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }
        StartNextWave();
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
