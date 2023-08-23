using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Liminal.SDK.VR;
using Liminal.SDK.VR.Input;

public class GunShootDamage : MonoBehaviour
{
    public float minChargeTime = 0.5f;
    public float maxChargeTime = 1.5f;
    public float minDamage = 5f;
    public float maxDamage = 25f;

    private float chargeTimer;
    private bool isCharging;

    

    private void Update()
    {
        var primaryInput = VRDevice.Device.PrimaryInputDevice;

        if (primaryInput.GetButtonDown(VRButton.One) || Input.GetMouseButtonDown(0))
        {
            isCharging = true;
            chargeTimer = 0f;
        }

        if (isCharging)
        {
            chargeTimer += Time.deltaTime;

            if (chargeTimer >= minChargeTime)
            {
                // charging animation
            }

            if (chargeTimer >= maxChargeTime)
            {
                FireLaser(chargeTimer);
                chargeTimer = 0f;
                isCharging = false;
            }
        }
    }

    private void FireLaser(float chargeTime)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                float damage = Mathf.Lerp(minDamage, maxDamage, (chargeTime - minChargeTime) / (maxChargeTime - minChargeTime));


                enemyHealth.Damage(damage, DamageType.Gun);
            }
        }
    }

}
