using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Health health; //reference to health script component

    public Transform fillBar; // The transform representing the health bar's fill

    private Color maxHealthColor = new Color(0f, 1f, 0f); // green 
    private Color minHealthColor = new Color(1f, 0f, 0f); // red 

    private void Start()
    {

        health = GetComponent<Health>();

        // Make sure a reference to the Health component is provided
        if (health == null)
        {
            Debug.LogWarning("No Health component assigned to HealthBarAboveEnemy.");
            enabled = false; // Disable this script to prevent errors
            return;
        }

        // Subscribe to the health change event
        health.OnHealthChanged += HandleHealthChanged;

        // Set the initial health bar color
        UpdateHealthBarColor();
    }

    private void OnDestroy()
    {
        health = GetComponent<Health>();

        // Unsubscribe from the OnDamaged event when the GameObject is destroyed
        if (health != null)
        {
            health.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        // Update the health bar color when damage is taken
        UpdateHealthBarColor();
    }

    private void UpdateHealthBarColor()
    {
        // Calculate the fill amount based on current health
        float fillAmount = health.currentHealth / health.maxHealth;

        // Lerp between maxHealthColor (green) and minHealthColor (red) based on fill amount
        Color lerpedColor = Color.Lerp(minHealthColor, maxHealthColor, fillAmount);

        // Apply the lerped color to the health bar's fill
        fillBar.GetComponent<Renderer>().material.color = lerpedColor;
    }
}
