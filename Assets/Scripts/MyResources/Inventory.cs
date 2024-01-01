using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private List<UpgradeData> upgrades;

    private void Awake()
    {
        Instance = this;
    }

    public List<UpgradeData> GetAllUpgrades()
    {
        return upgrades;
    }
}
