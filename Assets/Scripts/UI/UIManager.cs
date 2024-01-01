using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject inGamePanel;
    public GameObject gameOverPanel;

    public Slider xpSlider;

    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI currentLevelText;

    // Waves
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI currentTimeText;
    public GameObject levelUpContent;
    public GameObject finishedWaveContent;

    public GameObject upgradePanel;

    //Upgrades Prefabs
    public GameObject itemCardPrefab;
    public GameObject statPrefab;
    public GameObject effectPrefab;

    public Color positiveColor;
    public Color negativeColor;

    public Transform cardsContainer;

    private void Awake()
    {
        Instance = this;
    }
}
