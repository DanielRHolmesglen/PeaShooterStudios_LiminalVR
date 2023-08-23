using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth: Health
{
    [Header("SwordArmour")]
    public float swordMultiplier = 1;
    

    [Header("GunArmour")]
    public float gunMultipiler = 1;
  

    public override void Damage(float amount, DamageType type)
    {
        //goes through damagetype types so we can use different multipliers for them
        switch (type)
        {
            //if the enum is Gun or Sword, then multiply it by that multiplier
            case DamageType.Sword:
                currentHealth -= amount * gunMultipiler;
                break;

            case DamageType.Gun:
                currentHealth -= amount * gunMultipiler;
                break;
            
            //if the enum isnt sword or gun, which it shouldnt, give us a message
            default:
                Debug.Log("damage type was not sword or gun");
                break;
        }
    }

}
