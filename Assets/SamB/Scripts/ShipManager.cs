using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Is in charge of managing the ship the player is defending, and 'resetting' or 'ending the game' with defeat if its destroyed.
/// </summary>

public class ShipManager : MonoBehaviour
{
    //static references for other scripts to use
    public static Transform player; //Where the ship is.
    public static Vector3 playerPosition; // ships's Vector3 position+
    public static PlayerHealth playerHealth; // reference to ships's health for all scripts to easily use 
    public static Collider[] PlayerColliders; //static reference for ship generator colliders, mainly for enemyMovement and projectiles
    public static GameObject playerObject; //used for enemymovement

    public Collider[] setPlayerColliders; //assign them in inspector

    public StartingSequence startingSequence; //used for some Ui stuff
    public Text hologramText;
    public GameObject AIModel;
    public Image fadeImage; //used to fade screen to black

    public float resetDelay = 3f; 
    public AudioSource audioSource;
    public AudioClip lossSound;
    public AudioClip winSound;

    public WaveManager waveManager;

    public ParticleSystem shipExplosionParticle; //used for death
    //Also would add in particles for victory 



    private void Awake()
    {
        player = GetComponent<Transform>();
        PlayerColliders = setPlayerColliders;
        playerObject = gameObject;
        playerHealth = GetComponent<PlayerHealth>();
        playerPosition = transform.position;

        if (playerHealth != null)
        {
            // Subscribe to the health change event
            playerHealth.OnHealthChanged += HandleHealthChanged;
        }
    }

    
    private void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        if (playerHealth.currentHealth <= 0f)   // Player health is zero or negative, reload game
        {
            waveManager.StartCoroutine(waveManager.DestroyEnemies());
            waveManager.StopCoroutine(waveManager.SpawnWaves());

            AIModel.SetActive(true);
            hologramText.gameObject.SetActive(true);
            hologramText.text = "The ship has lost too much health. We need to repair and try again.";
            //Debug.Log("Ship has been destroyed");

            startingSequence.fadeDuration = 7;
            startingSequence.StartCoroutine(startingSequence.FadeToBlack());

            ParticleSystem lossParticle = Instantiate(shipExplosionParticle, transform.position, Quaternion.identity);
            Destroy(lossParticle.gameObject, lossParticle.main.duration);

            audioSource.PlayOneShot(winSound);

            Invoke("ReloadLevel", 7);

        }
    }

    public void OnVictory()
    {
        waveManager.StartCoroutine(waveManager.DestroyEnemies());

        hologramText.gameObject.SetActive(true);
        hologramText.text = "You did it! You defeated all the bugs!";
        //Debug.Log("Waves have ended!");

        audioSource.PlayOneShot(lossSound);

        startingSequence.fadeDuration = 4;
        startingSequence.StartCoroutine(startingSequence.FadeToBlack());

        Invoke("ReloadLevel", 5);

    }

    private void OnDestroy()
    {

        // Unsubscribe from the health change event when the Ship is destroyed
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= HandleHealthChanged;
        }
    }

    public void ReloadLevel()
    {
        StartingSequence.isReloaded = true;

        hologramText.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }



}
