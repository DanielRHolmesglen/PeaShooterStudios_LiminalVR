using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Mommy health script that is inherited by all other health scripts. Responsible for max hp, current hp, Damage, healing and Dying.
/// </summary>
public class Health : MonoBehaviour
{

    [Header("Attributes")]
    public float maxHealth = 50f;
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void Damage(float amount, DamageType type)
    {
        //checking if health is ,= 0. If it isn't do the damage, if it is instead Destroy the object. 
        if (currentHealth >= 0)
        {
            currentHealth = -amount;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public virtual void Die()
    {
        // Remove from scene
        Invoke("Destroy", 1);
    }

    //destroy gameobject
    private void Destroy()
    {
        Destroy(gameObject);

    }




}
