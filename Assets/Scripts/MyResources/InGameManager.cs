using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public GameObject playerGameObject;
    public UIManager uiManager;
    public WaveManager waveManager;
    public ResourceManager resourceManager;

    private GameObject player;
    private Inventory inventory;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        inventory = Inventory.Instance;
        RestartGame();
        // Play background music
        AudioManager.PlaySound(AudioManager.Sound.BackgroundMusic);
    }

    public void RestartGame()
    {
        player = Instantiate(playerGameObject, Vector3.zero, Quaternion.identity);
        player.GetComponent<PlayerWeaponController>().SetupStartingWeapons();
        uiManager.gameOverPanel.SetActive(false);
        CleanScreen();
        waveManager.ResetWaves();
        resourceManager.ResetResources();
        ResumeGame();
    }

    public void GenerateUpgradeOptions()
    {
        var randomOptions = GetRandomOptions(inventory.GetAllUpgrades(), 3);

        foreach (var item in randomOptions)
        {
            var card = Instantiate(uiManager.itemCardPrefab, transform.position, Quaternion.identity, uiManager.cardsContainer);
            SetupCard(card, item);
            SetupStats(item, card.transform.Find("Stats"), card.transform.Find("Effects"));
        }
    }

    private void SetupCard(GameObject card, UpgradeData item)
    {
        card.transform.GetChild(0).GetComponent<Image>().sprite = item.upgradeIcon;
        card.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"- {item.upgradeType} -";
        card.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.upgradeName;
        card.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = item.upgradeTier.ToString();
    }

    private void SetupStats(UpgradeData item, Transform statContainer, Transform effectContainer)
    {
        foreach (Pair<Stat, float> stat in item.itemStats)
        {
            var statObj = Instantiate(uiManager.statPrefab, transform.position, Quaternion.identity, statContainer);
            var firstChildText = statObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var secondChildText = statObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            var symbol = "";
            if (stat.first is Stat.MoveSpeed || stat.first is Stat.Damage
                || stat.first is Stat.Critic_Chance || stat.first is Stat.Dodge)
            {
                symbol = "%";
            }

            firstChildText.color = uiManager.positiveColor;
            firstChildText.text = $"+{stat.second}{symbol}";
            secondChildText.text = stat.first.ToString();
        }

        foreach (Pair<Stat, float> stat in item.itemSideEffects)
        {
            var statObj = Instantiate(uiManager.statPrefab, transform.position, Quaternion.identity, statContainer);
            var firstChildText = statObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var secondChildText = statObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            var symbol = "";
            if (stat.first is Stat.MoveSpeed || stat.first is Stat.Damage
                || stat.first is Stat.Critic_Chance || stat.first is Stat.Dodge)
            {
                symbol = "%";
            }

            firstChildText.color = uiManager.negativeColor;
            firstChildText.text = $"-{stat.second}{symbol}";
            secondChildText.text = stat.first.ToString();
        }


        if (item.effect != null && item.effect.Length > 0 && effectContainer != null)
        {
            var effect = Instantiate(uiManager.effectPrefab, transform.position, Quaternion.identity, effectContainer);
            effect.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.effect;
        }
    }

    private List<UpgradeData> GetRandomOptions(List<UpgradeData> sourceList, int count)
    {
        return sourceList.OrderBy(x => Random.value).Take(count).ToList();
    }

    public void GameOver()
    {
        uiManager.gameOverPanel.SetActive(true);
        PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void CleanScreen()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        var marks = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Spawn Mark(Clone)");
        foreach (GameObject spawnMark in marks)
        {
            Destroy(spawnMark);
        }

        var playerBullets = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Player Projectile(Clone)");
        foreach (GameObject bullet in playerBullets)
        {
            Destroy(bullet);
        }

        var souls = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "BlessingFragment(Clone)");
        foreach (GameObject soul in souls)
        {
            Destroy(soul);
        }
    }
}
