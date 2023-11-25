using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INCLUDE THE SCENE MANAGER
using UnityEngine.SceneManagement;
public class RestartExample : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RestartLevel());
        }
    }

    //run a coroutine on game over so that you can sequence some events, like audio and screen effects.
    IEnumerator RestartLevel()
    {
        //fade out your screen using the same method as your intro
        //show any ui or sound
        yield return new WaitForSeconds(2f); //remember you can easily pause between behaviours

        //load the current scene again, effectively restarting the game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
