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
    public Text boomGunText;
    public Text pewGunText;

    public GameObject AIModel;
    public GameObject box;
    public GameObject bugModel;
    public GameObject clumpedBugs;
    public GameObject singleBugs;
    public GameObject shipModel;
    public GameObject pewGunModel;
    public GameObject boomGunModel;

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
        AIModel.SetActive(true);
        hologramText.gameObject.SetActive(true);
        hologramText.text = "Initialising...";
        yield return new WaitForSeconds(2.0f);
        hologramText.text = "Glad to see you're awake, but looks like you need a refresher.";
        yield return new WaitForSeconds(4.0f);
        hologramText.text = "Well good news is that we found the AI that was broadcasting the frequency.";
        yield return new WaitForSeconds(4.0f);

        //Show ship
        shipModel.SetActive(true);
        hologramText.gameObject.SetActive(true);
        hologramText.text = "Bad news is the bugs are not very happy we're here.";
        yield return new WaitForSeconds(5.0f);
        hologramText.text = "We must defend the generators until this colonies AI can upload itself onboard our ship, then we can get the hell out of here.";
        yield return new WaitForSeconds(6.0f);
        shipModel.SetActive(false);


        bugModel.SetActive(true);
        hologramText.text = "These are the locals, hostile alien bug creatures that are trying to destroy the ship to stop us from escaping.";
        yield return new WaitForSeconds(6.0f);
        bugModel.SetActive(false);

        // Showing and hiding guns. Showing what they do and theyr keybinds
        hologramText.text = "These are your currently equipped weapons. You will need to use them to keep the bugs off our ship. Try them out on these initial bugs!";
        boomGunModel.SetActive(true);
        pewGunText.gameObject.SetActive(true);
        boomGunText.gameObject.SetActive(true);
        pewGunModel.SetActive(true);
        yield return new WaitForSeconds(8.0f);
        boomGunModel.SetActive(false);
        pewGunModel.SetActive(false);
        pewGunText.gameObject.SetActive(false);
        boomGunText.gameObject.SetActive(false);

        //showing and hiding bugs to show how to shoot guns 
        clumpedBugs.SetActive(true);
        hologramText.text = "Hold down your right button to charge your cannon. Release it to cause a big explosion and hit those clumped bugs.";
        yield return new WaitForSeconds(5.0f);
        clumpedBugs.SetActive(false);
        singleBugs.SetActive(true);
        hologramText.text = "Now press your left button to fire your pistol. Use it to shoot each of those far apart bugs.";
        yield return new WaitForSeconds(5.0f);
        singleBugs.SetActive(false);

        


        //make screen black as screeches play
        hologramText.text = "Here they come! Time for you to slay some bugs.";
        audioSource.PlayOneShot(bugScreeching);

        fadeDuration = 4f;
        StartCoroutine(FadeToBlack());
        yield return new WaitForSeconds(4.0f);
        hologramText.gameObject.SetActive(false);


        hologramText.gameObject.SetActive(false);
        fadeDuration = 3;
        StartCoroutine(FadeIn());
        box.SetActive(false);
        AIModel.SetActive(false);


        //start wave spawning as the game now 'starts' and tutorial ends
        tutorialOn = false;
        waveManager.StartCoroutine(waveManager.SpawnWaves());
        //Destroy(gameObject);

    }

    public IEnumerator FadeIn()
    {
        // Set the initial alpha of the black screen to 0% (AKA black)
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

    public IEnumerator FadeToBlack()
    {
        // Set the initial alpha of the black screen to 100% (AKA see through)
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
        AIModel.SetActive(false);
        pewGunModel.SetActive(false);
        boomGunModel.SetActive(false);
        pewGunText.gameObject.SetActive(false);
        boomGunText.gameObject.SetActive(false);
        shipModel.SetActive(false);
        bugModel.SetActive(false);
        //make sure screen is not 'fading in' still (setting alpha to 100% so it's see through)
        Color startColor = blackScreenImage.color;
        startColor.a = 100;



        tutorialOn = false;
        box.SetActive(false);
        waveManager.StartCoroutine(waveManager.SpawnWaves());
        //Destroy(gameObject);

    }


}
