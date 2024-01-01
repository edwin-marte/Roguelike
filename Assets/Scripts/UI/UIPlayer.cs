using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    public static UIPlayer Instance;

    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        Instance = this;
    }

    public void SetMaxHealth(float maxHealth, float currentHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
        UIManager.Instance.currentHealthText.text = $"{currentHealth} / {maxHealth}";
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        healthSlider.value = currentHealth;
        UIManager.Instance.currentHealthText.text = $"{currentHealth} / {maxHealth}";
    }
}
