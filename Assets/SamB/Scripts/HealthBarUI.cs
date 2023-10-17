using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health health; // Reference to the player's health script
    public Image healthBar; // Reference to the UI Image of bar

    public Color minHealthColor = Color.red; // Color when health is low
    public Color maxHealthColor = Color.green; // Color when health is high


    private void Start()
    {

        // Make sure a reference to the health script is provided
        if (health == null)
        {
            Debug.LogWarning("No health component assigned to HealthBar.");
            enabled = false; // Disable this script to prevent errors
            return;
        }

        // Subscribe to the player's health change event
        health.OnHealthChanged += UpdateHealthBar;

        // Set the initial health bar value
        UpdateHealthBar(health.currentHealth, health.maxHealth);
    }

   // Method to update the health bar based on the player's health
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Calculate the health percentage (value between 0 and 1)
        float healthPercent = currentHealth / maxHealth;

        // Interpolate between minHealthColor (red) and maxHealthColor (green) based on health percentage
        Color lerpedColor = Color.Lerp(minHealthColor, maxHealthColor, healthPercent);

        // Update the health bar's fill color
        healthBar.color = lerpedColor;

        // Adjust the health bar's fill amount based on the percentage
        healthBar.fillAmount = healthPercent;

      
    }

    private void OnDestroy()
    {
        // Unsubscribe from the player's health change event when this GameObject is destroyed
        if (health != null)
        {
            health.OnHealthChanged -= UpdateHealthBar;
        }
    }

}
