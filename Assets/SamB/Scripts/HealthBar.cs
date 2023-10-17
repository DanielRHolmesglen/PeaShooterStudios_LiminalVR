using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public  Transform bar; //the bar that is being scaled to show hp left
 
    private Health health; //reference to health script component

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
        else
        {
            // Set the initial health bar color/value
            UpdateHealthBar();
        }

        // Subscribe to the health change event
        health.OnHealthChanged += HandleHealthChanged;
        
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        // Update the health bar color when damage is taken
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        
        // Calculate the fill amount based on current health
        float fillAmount = health.currentHealth / health.maxHealth;

        // set health bar fill
        bar.localScale = new Vector3(fillAmount, 1f, 1f);

        // Lerp between maxHealthColor (green) and minHealthColor (red) based on fill amount
        Color lerpedColor = Color.Lerp(minHealthColor, maxHealthColor, fillAmount);

        // Apply the lerped color to the health bar's fill
        bar.GetComponentInChildren<SpriteRenderer>().color = lerpedColor;
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
}
