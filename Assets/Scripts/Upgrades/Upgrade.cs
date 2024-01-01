using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "ScriptableObjects/UpgradeData")]
public class UpgradeData : ScriptableObject
{
    public Sprite upgradeIcon;
    public string upgradeName;
    public ItemType upgradeType;
    public Tier upgradeTier;
    public string effect;
    public List<Pair<Stat, float>> itemStats;
    public List<Pair<Stat, float>> itemSideEffects;
}

public enum ItemType
{
    Blessing,
    Projectile
}

public enum Tier
{
    I,
    II,
    III,
    IV
}