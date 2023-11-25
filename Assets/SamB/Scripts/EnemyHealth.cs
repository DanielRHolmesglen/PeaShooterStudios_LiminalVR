using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Inherits from Health. Specifically used for bugs and their health. 
/// </summary>
public class EnemyHealth: Health
{
    //bools to determine what enemy type, for death effects
    public bool isSoldier;
    public bool isArtillery;
    public bool isDrifter;
    public bool isExploder;

    Animator anim; 

    private ColorChange ColorChange; // UNUSED. Reference to the ColorChange script

    [Header("SwordArmour")]
    public float swordMultiplier = 1; // UNUSED. sword damage multiplier. 


    [Header("GunArmour")]
    public float gunMultipiler = 1;

    public ParticleSystem deathParticleSoldier;
    public ParticleSystem deathParticleArtillery;
    public ParticleSystem deathParticleDrifter;



    protected override void Awake()
    {
        base.Awake(); //invoke the parent(base) class's start first, then add the below stuff
        ColorChange = GetComponent<ColorChange>();

        anim = GetComponentInChildren<Animator>();


    }


    //we are not calling base as dont want to do damage twice, we are just overriding it with the gun/sword multiplier
    public override void Damage(float amount, DamageType type)
    {
        RaiseOnHealthChanged(currentHealth, maxHealth);

        switch (type)   //go through damagetype types so we can use different multipliers for 
        {
                //Do damage and  multiply amount by the correct multiplier
                case DamageType.Sword:
                currentHealth -= amount * swordMultiplier;
                OnDamageTaken();
                break;
                    
                     

                case DamageType.Gun:
                    currentHealth -= amount * gunMultipiler;
                    OnDamageTaken();
                    break;


                
            //if the enum isnt sword or gun, debug
            default:
                    Debug.Log("damage type was not sword or gun");
                    break;
        }

        if (currentHealth <= 0) //if it should die
        {
            WaveManager.currentEnemies.Remove(gameObject);

            if (isArtillery) //different death animations/effects for each unit type
            {
                anim.Play("ArtilleryDEATH");
                //anim.Play("Death");
                ParticleSystem deathEffectArtillery = Instantiate(deathParticleArtillery, transform.position, Quaternion.identity);

            }
            else if (isSoldier)
            {
                anim.Play("ScorpionDEATH");
                //anim.Play("Death");
                ParticleSystem deathEffectSolider = Instantiate(deathParticleSoldier, transform.position, Quaternion.identity);

            }
            else if (isExploder)
            {
                anim.Play("ScorpionDEATH");
                //anim.Play("Death");
                ParticleSystem deathEffectSoldier = Instantiate(deathParticleSoldier, transform.position, Quaternion.identity);

            }
            else if (isDrifter)
            {
                anim.Play("DrifterDEATH");
                //anim.Play("Death");
                ParticleSystem deathEffectDrifter = Instantiate(deathParticleArtillery, transform.position, Quaternion.identity);
            }
            else Debug.LogWarning("NoEnemyTypeFound");
            

            Die();

        }

    }

    //when damage is taken, was used for 'flash red' when things were damaged. unused currently. 
    public void OnDamageTaken()
    {
        ColorChange.DamagedFlash();
    }

}
