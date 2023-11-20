using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UNUSED: controls the 'filling up' of the right hand gun "charge up" bar on the canvas
/// </summary>
public class PewGunCharge : MonoBehaviour
{
    public AudioClip laserChargeSound;
    private AudioSource audioSource;

    public Image bar; // Reference to the UI Image of bar
    public GameObject gunObject; //reference to the gun with the guncontroller script

    public Color minChargeColor = new Color(0.4f, 0.3f, 0.25f); 
    public Color maxChargeColor = new Color(0.2f, 0.6f, 0.55f);

    public BoomBoomGunController gunController;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = laserChargeSound;

        // Make sure a reference to the Health component is provided
        if (gunController == null)
        {
            Debug.LogWarning("No Guncontroller found");
            enabled = false; // Disable this script to prevent errors
            return;
        }


    }
    /* MORE UNUSED CHARGE STUFF
    //could make this an event like health changing, but update is much easier for the moment
    private void Update()
    {        

            // Calculate the fill amount based on current charge time
            float fillAmount = gunController.chargeTimer / gunController.maxChargeTime;

            // Lerp between  (green) and  (red) based on fill amount
            Color lerpedColor = Color.Lerp(minChargeColor, maxChargeColor, fillAmount);

            // Apply the lerped color to the health bar's fill
            bar.color = lerpedColor;

            //fill the charge bar
            bar.fillAmount = fillAmount;

        
        if (gunController.isCharging && audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
        else if (gunController.isCharging == false)
        {
            audioSource.Stop();
        }
        

      }
        */


}
