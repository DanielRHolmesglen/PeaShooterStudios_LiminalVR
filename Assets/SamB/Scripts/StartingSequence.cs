using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartingSequence: MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip shipLanding;
    public AudioClip bugScreeching;

    public Text hologramText;
    public Text gunText;
    public Text swordText;

    public GameObject planetModel;
    public GameObject box;
    public GameObject bugModel;
    public GameObject shipModel;
    public GameObject gunModel;
    public GameObject swordModel;

    public Image blackScreenImage;

    public bool tutorialOn;
    public KeyCode skipKey = KeyCode.Space;
    public WaveManager waveManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shipLanding;


        StartCoroutine(PlayStartingSequence());
    }

    private void Update()
    {
        // Check if the skip key is pressed
        if (Input.GetKeyDown(skipKey))
        {
            // If the skip key is pressed, skip to the end of the sequence
            StopAllCoroutines();
            EndSequence();
        }
    }

    private IEnumerator PlayStartingSequence()
    {
        tutorialOn = true;

        // Set the initial alpha of the black screen to 0
        Color startColor = blackScreenImage.color;
        startColor.a = 0;
        blackScreenImage.color = startColor;

        //setting fade duration for fades in/outs
        float fadeDuration = 3.0f;
        float startTime = Time.time;

        // Decrease alpha over 2 seconds to fade in as you wake up
        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            startColor.a = Mathf.Lerp(1, 0, progress);
            blackScreenImage.color = startColor;
            yield return null;
        }

        // Play ship landing/crashing sounds
        audioSource.PlayOneShot(shipLanding);
        yield return new WaitForSeconds(3.0f);

        // Show Ai with initial intro/backrgound
        planetModel.SetActive(true);
        hologramText.gameObject.SetActive(true);
        hologramText.text = "Initialising...";
        yield return new WaitForSeconds(2.0f);
        hologramText.text = "Glad to see you're awake, and more importantly, unharmed.";
        yield return new WaitForSeconds(3.0f);
        hologramText.text = "Something hit us as we were trying to land, we had a very rocky landing.";
        yield return new WaitForSeconds(3.0f);
        hologramText.gameObject.SetActive(false);
        planetModel.SetActive(false);

        //Show ship
        shipModel.SetActive(true);
        hologramText.gameObject.SetActive(true);
        hologramText.text = "Behind you is our ship, we must defend it until this planets AI can upload itself on board.";
        yield return new WaitForSeconds(4.0f);
        shipModel.SetActive(false);

        // Showing and hiding gun/sword to show what they do and theyr keybinds
        hologramText.text = "These are your currently equipped weapons. You will need to use them to keep the bugs off our ship.";
        gunModel.SetActive(true);
        gunText.gameObject.SetActive(true);
        swordText.gameObject.SetActive(true);
        swordModel.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        swordModel.SetActive(false);
        gunModel.SetActive(false);
        gunText.gameObject.SetActive(false);
        swordText.gameObject.SetActive(false);


        bugModel.SetActive(true);
        hologramText.text = "These are the locals, hostile alien bug creatures that are trying to destroy the ship to stop us from escaping.";
        yield return new WaitForSeconds(5.0f);
        bugModel.SetActive(false);


        //make screen black as screeches play
        hologramText.text = "Here they come! Time for you to squash these bugs.";
        audioSource.PlayOneShot(bugScreeching);

        startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            startColor.a = Mathf.Lerp(0, 1, progress);
            blackScreenImage.color = startColor;
            yield return null;
        }

        hologramText.gameObject.SetActive(false);

        // Decrease black image alpha over 1 second as game now sarts
        box.SetActive(false);

        startTime = Time.time;
        fadeDuration = 1.0f;

        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            startColor.a = Mathf.Lerp(1, 0, progress);
            blackScreenImage.color = startColor;
            yield return null;
        }

        tutorialOn = false;
        //start wave spawning as the game now 'starts'
        waveManager.StartCoroutine(waveManager.SpawnWaves());
        Destroy(gameObject);

    }


    private void EndSequence()
    {
        //make sure everything is actually off (as you end the coroutine early). Could also just destroy the game objects.
        hologramText.gameObject.SetActive(false);
        planetModel.SetActive(false);
        swordModel.SetActive(false);
        gunModel.SetActive(false);
        gunText.gameObject.SetActive(false);
        swordText.gameObject.SetActive(false);
        shipModel.SetActive(false);
        bugModel.SetActive(false);



        tutorialOn = false;
        box.SetActive(false);
        waveManager.StartCoroutine(waveManager.SpawnWaves());
        Destroy(gameObject);


    }


}
