using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float swordDamage = 20f;

    //Dont think we can use layers? So have to use a tag.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                //do damage, and pass in the sword enum so enemyhealth knows what type of damage it is 
                health.Damage(swordDamage, DamageType.Sword);
            }
        }
    }
}
