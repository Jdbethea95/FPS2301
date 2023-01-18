using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void Settings()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = GameManager.instance.settingsMenu;
        GameManager.instance.activeMenu.SetActive(true);

        //update sensitivity slider position with current value
        GameManager.instance.xSenSlider.value = GameManager.instance.playerScript.cam.XSen;
        GameManager.instance.ySenSlider.value = GameManager.instance.playerScript.cam.YSen;
    }

    public void SettingsBack()
    {
        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = GameManager.instance.pauseMenu;
        GameManager.instance.activeMenu.SetActive(true);        
    }

    public void SettingsSave()
    {
        GameManager.instance.playerScript.cam.XSen = GameManager.instance.xSenSlider.value;
        GameManager.instance.playerScript.cam.YSen = GameManager.instance.ySenSlider.value;

        SettingsBack();
    }

    public void ReturnToMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("MainMenu");
    }

    public void TempPlay()
    {       
        SceneManager.LoadScene("BuildingLevel");
        GameManager.instance.UnPause();
    }
}
