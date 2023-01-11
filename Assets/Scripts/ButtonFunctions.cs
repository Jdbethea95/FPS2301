using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    public void Resume() 
    {
        GameManager.instance.UnPause();
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
    }

    public void Restart() 
    {
        GameManager.instance.UnPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
