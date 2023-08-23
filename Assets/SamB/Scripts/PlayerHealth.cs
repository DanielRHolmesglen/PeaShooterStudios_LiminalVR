using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using a playerhealth script so if we want to give the player "armors" and other health effect later on we can

public class PlayerHealth : Health
{ 
    public override void Damage(float amount, DamageType type)
    {
        currentHealth -= amount;
    }
}
