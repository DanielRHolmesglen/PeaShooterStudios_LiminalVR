using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public float swordDamage = 20f;

    //Dont think we can use layers? So have to use a tag.
    private void OnTriggerEnter(Collider other)
    {
        //looking for colliders with the EnemeyHealth script
        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
        {
            EnemyHealth health = other.GetComponent<EnemyHealth>();
            if (health != null)
            {
                //do damage, and pass in the sword enum so enemyhealth knows what type of damage it is 
                enemyHealth.Damage(swordDamage, DamageType.Sword);
            }
        }
    }
}
