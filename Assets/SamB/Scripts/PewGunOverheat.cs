using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomGunCharge : MonoBehaviour
{

    public Image bar; // Reference to the UI Image of bar
    public GameObject gunObject; //reference to the gun with the guncontroller script

    public Color minChargeColor = new Color(0.6f, 0.25f, 0f);
    public Color maxChargeColor = new Color(0.9f, 0.4f, 0f);

    public PewPewGunController gunController;

    private void Start()
    {
        // Make sure a reference to the guncontroller component is provided
        if (gunController == null)
        {
            Debug.LogWarning("No Guncontroller found");
            enabled = false; // Disable this script to prevent errors
            return;
        }


    }

    //could make this an event like health changing, but update is much easier for the moment
    private void Update()
    {        

            // Calculate the fill amount based on current charge time
            float fillAmount = gunController.currentHeat / gunController.overheatThreshold;

            // Lerp between  (green) and  (red) based on fill amount
            Color lerpedColor = Color.Lerp(minChargeColor, maxChargeColor, fillAmount);

            // Apply the lerped color to the health bar's fill
            bar.color = lerpedColor;

            //fill the charge bar
            bar.fillAmount = fillAmount;

        
    }


}
