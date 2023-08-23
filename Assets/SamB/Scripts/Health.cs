using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    

    [Header("Mods")]
    public int RangedArmor;
    public int MeleeArmor;

    [Header("Attributes")]
    public float maxHealth = 50f;
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void Damage(float amount, DamageType type)
    {
        currentHealth = -amount;
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " has perished");
        // other death stuff
    }

    
}
