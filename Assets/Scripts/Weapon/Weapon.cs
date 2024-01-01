using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Sprite shopItemImage;
    [SerializeField] protected string shopItemName;
    [SerializeField] protected string shopItemDescription;
    [SerializeField] protected float shopItemCost;
    [SerializeField] private Projectile projectilePrefab;

    [SerializeField] private List<Pair<WeaponStat, float>> weaponStats;
    [SerializeField] private List<Pair<WeaponStat, float>> weaponSideEffects;

    public List<Pair<WeaponStat, float>> GetWeaponStats()
    {
        return weaponStats;
    }

    public List<Pair<WeaponStat, float>> GetWeaponSideEffects()
    {
        return weaponSideEffects;
    }

    public float GetWeaponStat(WeaponStat stat)
    {
        return weaponStats.FirstOrDefault(i => i.first == stat).second;
    }

    public Projectile GetProjectile()
    {
        return projectilePrefab;
    }

    public Sprite GetShopItemImage()
    {
        return shopItemImage;
    }

    public string GetShopItemName()
    {
        return shopItemName;
    }

    public string GetShopItemDescription()
    {
        return shopItemDescription;
    }

    public float GetShopItemCost()
    {
        return shopItemCost;
    }
}
