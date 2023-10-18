using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Mommy health script that is inherited by all other health scripts. Responsible for max hp, current hp, Damage, healing and Dying.
/// </summary>
public class Health : MonoBehaviour
{

    [Header("Attributes")]
    public float maxHealth = 50f;
    public float currentHealth;

    //event to notify other scripts when health changes 
    public event Action<float, float> OnHealthChanged;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void Damage(float amount, DamageType type)
    {
        //checking if health is 0. If it isn't do the damage, if it is instead Destroy the object. 
        if (currentHealth > 0)
        {
            currentHealth = currentHealth - amount;

            // Notify subscribers that health has changed
            RaiseOnHealthChanged(currentHealth, maxHealth);

        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        // Notify subscribers that health has changed
        RaiseOnHealthChanged(currentHealth, maxHealth);
    }

    // Remove from scene
    public virtual void Die()
    {
        Invoke("Destroy", (1/2));
    }

    //destroy gameobject
    private void Destroy()
    {
        Destroy(gameObject);

    }

    protected void RaiseOnHealthChanged(float currentHealth, float maxHealth)
    {
        //invoke the event to notify subscribers 
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }


}
