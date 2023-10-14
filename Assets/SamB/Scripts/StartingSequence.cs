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

    float fadeDuration;


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
        fadeDuration = 4.5f;
        StartCoroutine(FadeIn());

        // Play ship landing/crashing sounds
        audioSource.PlayOneShot(shipLanding);
        yield return new WaitForSeconds(4.5f);

        // Show Ai with initial intro/backrgound
        planetModel.SetActive(true);
        hologramText.gameObject.SetActive(true);
        hologramText.text = "Initialising...";
        yield return new WaitForSeconds(2.0f);
        hologramText.text = "Glad to see you're awake, and more importantly, unharmed.";
        yield return new WaitForSeconds(4.0f);
        hologramText.text = "Something hit us as we were trying to land, we had a very rocky landing.";
        yield return new WaitForSeconds(4.0f);
        hologramText.gameObject.SetActive(false);
        planetModel.SetActive(false);

        //Show ship
        shipModel.SetActive(true);
        hologramText.gameObject.SetActive(true);
        hologramText.text = "Behind you is our ship, we must defend it until this planets AI can upload itself on board.";
        yield return new WaitForSeconds(6.0f);
        shipModel.SetActive(false);

        // Showing and hiding gun/sword to show what they do and theyr keybinds
        hologramText.text = "These are your currently equipped weapons. You will need to use them to keep the bugs off our ship.";
        gunModel.SetActive(true);
        gunText.gameObject.SetActive(true);
        swordText.gameObject.SetActive(true);
        swordModel.SetActive(true);
        yield return new WaitForSeconds(8.0f);
        swordModel.SetActive(false);
        gunModel.SetActive(false);
        gunText.gameObject.SetActive(false);
        swordText.gameObject.SetActive(false);


        bugModel.SetActive(true);
        hologramText.text = "These are the locals, hostile alien bug creatures that are trying to destroy the ship to stop us from escaping.";
        yield return new WaitForSeconds(6.0f);
        bugModel.SetActive(false);


        //make screen black as screeches play
        hologramText.text = "Here they come! Time for you to slay some bugs.";
        audioSource.PlayOneShot(bugScreeching);

        fadeDuration = 3f;
        StartCoroutine(FadeOutBlack());
        yield return new WaitForSeconds(3.0f);

        hologramText.gameObject.SetActive(false);
        fadeDuration = 2;
        StartCoroutine(FadeIn());
        box.SetActive(false);

        //start wave spawning as the game now 'starts' and tutorial ends
        tutorialOn = false;
        waveManager.StartCoroutine(waveManager.SpawnWaves());
        //Destroy(gameObject);

    }

    public IEnumerator FadeIn()
    {
        // Set the initial alpha of the black screen to 0
        Color startColor = blackScreenImage.color;
        startColor.a = 0;
        blackScreenImage.color = startColor;

        float startTime = Time.time;

        startTime = Time.time;
        // Decrease black image alpha over 1 second (AKA remove black screen)
        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            startColor.a = Mathf.Lerp(1, 0, progress);
            blackScreenImage.color = startColor;
            yield return null;
        }
    }

    public IEnumerator FadeOutBlack()
    {
        // Set the initial alpha of the black screen to 100
        Color startColor = blackScreenImage.color;
        startColor.a = 100;
        blackScreenImage.color = startColor;

        float startTime = Time.time;

        //Increase alpha gradually to make screen black
        startTime = Time.time;
        while (Time.time - startTime < fadeDuration)
        {
            float progress = (Time.time - startTime) / fadeDuration;
            startColor.a = Mathf.Lerp(0, 1, progress);
            blackScreenImage.color = startColor;
            yield return null;
        }
    }

    private void EndSequence()
    {
        //make sure everything is actually off (as you end the coroutine early).
        hologramText.gameObject.SetActive(false);
        planetModel.SetActive(false);
        swordModel.SetActive(false);
        gunModel.SetActive(false);
        gunText.gameObject.SetActive(false);
        swordText.gameObject.SetActive(false);
        shipModel.SetActive(false);
        bugModel.SetActive(false);
        //make sure screen is not 'fading in' still
        Color startColor = blackScreenImage.color;
        startColor.a = 0;



        tutorialOn = false;
        box.SetActive(false);
        waveManager.StartCoroutine(waveManager.SpawnWaves());
        //Destroy(gameObject);

    }


}
