using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Primarily used for tracking purposes since we can't use tags or layers. 
/// Can easily give the player armors if need be as well.
/// </summary>

public class PlayerHealth : Health
{
    /* If we need some extra logic for when player/ship takes damage we can override, but probably wont
    public override void Damage(float amount, DamageType type)
    {
        currentHealth -= amount;
    }
    */
    
}
