using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthChangeColor : MonoBehaviour
{
    private Renderer objectRenderer;
    private Health health; //reference to health script component

    [Header("Color Settings")]
    public Color highTintColor = new Color(0f, 1f, 0f, 0.35f); 
    //public Color mediumTintColor = new Color(1f, 1f, 0f, 0.35f); 
    public Color lowTintColor = new Color(1f, 0f, 0f, 0.35f); 

    [Header("Health Settings")]
    public float maxHealth = 100f;

    private void Start()
    {
        // Get the Renderer component to change the object's color
        objectRenderer = GetComponent<Renderer>();

        // Find the Health component or script with the health event
        health = GetComponent<Health>();

        if (health != null)
        {
            // Subscribe to the health change event
            health.OnHealthChanged += HandleHealthChanged;

            // Initialize the object's color based on initial health
            HandleHealthChanged(health.currentHealth, maxHealth);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the health change event when the GameObject is destroyed
        if (health != null)
        {
            health.OnHealthChanged -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        // Calculate the health percentage
        float healthPercent = currentHealth / maxHealth;

        // Choose the appropriate color based on the health percentage
        Color targetColor = Color.Lerp(highTintColor, lowTintColor, healthPercent);

        // Apply the color to the object's renderer
        objectRenderer.material.color = targetColor;
    }
}