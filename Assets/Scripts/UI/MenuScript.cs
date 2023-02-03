using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("----- EventSystem -----")]
    [SerializeField] GameObject selectedSettings;
    [SerializeField] GameObject returnSelected;
    [SerializeField] GameObject plateSelected;
    [SerializeField] EventSystem system;

    [Header("----- menus -----")]
    [SerializeField] GameObject namePlate;
    [SerializeField] GameObject settings;

    [Header("----- menu items -----")]
    public Slider xBar;
    public Slider yBar;

    [Header("----- buttons -----")]
    [SerializeField] Button play;
    [SerializeField] Button load;
    [SerializeField] Button save;
    [SerializeField] Button quit;
    [SerializeField] Button scoreBoard;
    [SerializeField] Button settingsBtn;

    public void CallPlate()
    {
        if (ScoreManager.instance.playerName == "JZH" || ScoreManager.instance.playerName == "")
        {
            namePlate.SetActive(true);
            system.SetSelectedGameObject(plateSelected);
            DisableButtons();
        }
            
        else
        {
            SceneManager.LoadScene("Lobby");
            GameManager.instance.UnPause();
            GameManager.instance.isPaused = false;
        }
           
    }

    public void OpenSettings() 
    {
        yBar.value = SaveManager.instance.gameData.ySen;
        xBar.value = SaveManager.instance.gameData.xSen;
        settings.SetActive(true);
        DisableButtons();
        system.SetSelectedGameObject(selectedSettings);
    }

    public void CloseSettings() 
    {
        settings.SetActive(false);
        EnableButtons();
        system.SetSelectedGameObject(returnSelected);
    }

    public void SaveSettings() 
    {
        SaveManager.instance.gameData.ySen = yBar.value;
        SaveManager.instance.gameData.xSen = xBar.value;
        CloseSettings();
    }

    public void DisableButtons() 
    {
        play.enabled = false;
        load.enabled = false;
        save.enabled = false;
        quit.enabled = false;
        scoreBoard.enabled = false;
        settingsBtn.enabled = false;
    }

    public void EnableButtons() 
    {
        play.enabled = true;
        load.enabled = true;
        save.enabled = true;
        quit.enabled = true;
        scoreBoard.enabled = true;
        settingsBtn.enabled = true;
    }
}
