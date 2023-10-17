using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// responsible for making enimies flash red when thye are hit. Called by health scripts when a unit is damaged. 
/// </summary>
public class ColorChange : MonoBehaviour
{
    public Color damagedColor = Color.red;
    public float flashDuration = 0.2f;
    public Renderer enemyRenderer; // Reference to the Renderer component

    private Color originalColor;
    private bool isFlashing;

    private void Start()
    {
        // Get the original color from the Renderer
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }
    }

    public void DamagedFlash()
    {
        // Change the color to the damaged color
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = damagedColor;

            // Start the flashing coroutine
            if (!isFlashing)
            {
                StartCoroutine(FlashColor());
            }
        }
    }

    private IEnumerator FlashColor()
    {
        isFlashing = true;

        yield return new WaitForSeconds(flashDuration);

        // Reset the color to the original color
        enemyRenderer.material.color = originalColor;

        isFlashing = false;
    }
}
