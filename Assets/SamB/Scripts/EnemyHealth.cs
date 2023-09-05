using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth: Health
{
    private ColorChange ColorChange; // Reference to the ColorChange script

    [Header("SwordArmour")]
    public float swordMultiplier = 1; //sword damage multiplier


    [Header("GunArmour")]
    public float gunMultipiler = 1;

    protected override void Awake()
    {
        base.Awake(); //invoke the parent(base) class's start first, then add the below stuff
        ColorChange = GetComponent<ColorChange>();

    }


    //we are not calling base as dont want to do damage twice, we are just overriding it with the gun/sword multiplier
    public override void Damage(float amount, DamageType type)
    {
        

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            //goes through damagetype types so we can use different multipliers for them
            switch (type)
            {
                //Do damage and  multiply amount by the correct multiplier
                case DamageType.Sword:
                    currentHealth -= amount * swordMultiplier;
                    OnDamageTaken();
                    
                    if (currentHealth <= 0)
                    {
                        Die();
                    }
                    break;
                     

                case DamageType.Gun:
                    currentHealth -= amount * gunMultipiler;
                    OnDamageTaken();
                    if (currentHealth <= 0)
                    {
                        Die();
                    }
                    break;

                //if the enum isnt sword or gun, which it should be, give a debug message and do nothing
                default:
                    Debug.Log("damage type was not sword or gun");
                    break;
            }
        }

        // Raise (not invoke, as you cant invoke in a different class) the event to notify subscribers about the health change
        RaiseOnHealthChanged(currentHealth, maxHealth);
    }


    public void OnDamageTaken()
    {
        ColorChange.DamagedFlash();
    }

}
