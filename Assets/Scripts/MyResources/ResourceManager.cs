using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    [SerializeField] private List<Pair<Stat, float>> currentStats;
    private List<Pair<UpgradeData, float>> currentUpgrades;
    private List<Weapon> currentWeapons;
    private float currentSouls = 0.0f;
    private float levelSouls = 0.0f;
    private int currentLevel = 0;

    private void Awake()
    {
        Instance = this;
        currentUpgrades = new List<Pair<UpgradeData, float>>();
        currentWeapons = new List<Weapon>();
    }

    private void Start()
    {
        var xPRequired = (currentLevel + 3) * (currentLevel + 3);
        UIManager.Instance.xpSlider.maxValue = xPRequired;
        UIManager.Instance.xpSlider.value = 0f;
        UIManager.Instance.currentLevelText.text = $"LV. {currentLevel}";
    }

    public void AddWeapon(Weapon weapon)
    {
        currentWeapons.Add(weapon);
    }

    public void RemoveWeapon(Weapon weapon)
    {
        if (currentWeapons.Count <= 0) return;
        currentWeapons.Remove(weapon);
    }

    public void AddSouls(float amount)
    {
        currentSouls += amount;
        levelSouls += amount;

        UIManager.Instance.xpSlider.value = levelSouls;

        var xPRequired = (currentLevel + 3) * (currentLevel + 3);
        if (levelSouls > xPRequired)
        {
            currentLevel += 1;

            // Play level up effect
            AudioManager.PlaySound(AudioManager.Sound.LevelUp);

            levelSouls = 0f;

            UIManager.Instance.currentLevelText.text = $"LV. {currentLevel}";
            UIManager.Instance.xpSlider.maxValue = (currentLevel + 3) * (currentLevel + 3);
            UIManager.Instance.xpSlider.value = 0f;
        }
    }

    public void ResetSouls()
    {
        currentSouls = 0f;
        levelSouls = 0f;
        currentLevel = 0;

        UIManager.Instance.xpSlider.maxValue = (1 + 3) * (1 + 3);
        UIManager.Instance.xpSlider.value = 0f;
    }

    public void DecreaseSouls(float amount)
    {
        currentSouls -= amount;
    }

    public float GetCurrentSouls()
    {
        return currentSouls;
    }

    public List<Pair<Stat, float>> GetCurrentStats()
    {
        return currentStats;
    }

    public List<Pair<UpgradeData, float>> GetCurrentItems()
    {
        return currentUpgrades;
    }

    public List<Weapon> GetCurrentWeapons()
    {
        return currentWeapons;
    }

    public void ResetResources()
    {
        if (currentStats == null || currentUpgrades == null || currentWeapons == null)
            return;

        ResetStats();
        ResetSouls();
        currentUpgrades.Clear();
        currentWeapons.Clear();
        currentSouls = 0.0f;
    }

    private void ResetStats()
    {
        foreach (Pair<Stat, float> stat in currentStats)
        {
            stat.second = 0f;
        }
    }
}
