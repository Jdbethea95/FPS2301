using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Slider sfxBar;
    public Slider musicBar;
    public Slider menuMusicBar;
    public Slider menuSfxBar;


    [SerializeField] TextMeshProUGUI sfxTxt;
    [SerializeField] TextMeshProUGUI musicTxt;
    [SerializeField] TextMeshProUGUI menuMusicTxt;
    [SerializeField] TextMeshProUGUI menuSfxTxt;

    [Header("----- buttons -----")]
    [SerializeField] Button play;
    [SerializeField] Button load;
    [SerializeField] Button save;
    [SerializeField] Button quit;
    [SerializeField] Button scoreBoard;
    [SerializeField] Button settingsBtn;
   
    public void CallPlate()
    {
        ScoreManager.instance.PlayChime();

        if (ScoreManager.instance.playerName == "JZH" || ScoreManager.instance.playerName == "")
        {
            namePlate.SetActive(true);
            system.SetSelectedGameObject(plateSelected);
            DisableButtons();
        }
            
        else
        {
            SceneManager.LoadScene("Lobby");
        }
           
    }

    public void OpenSettings() 
    {
        ScoreManager.instance.PlayChime();
        settings.SetActive(true);

        #region ValueUpdate
        yBar.value = SaveManager.instance.gameData.ySen;
        xBar.value = SaveManager.instance.gameData.xSen;
        sfxBar.value = SaveManager.instance.gameData.sfxVol;
        musicBar.value = SaveManager.instance.gameData.musicVol;
        menuSfxBar.value = SaveManager.instance.gameData.menuSfxVol;
        menuMusicBar.value = SaveManager.instance.gameData.menuMusicVol; 
        #endregion

        DisableButtons();
        system.SetSelectedGameObject(selectedSettings);
    }

    public void CloseSettings() 
    {
        ScoreManager.instance.PlayChime();

        settings.SetActive(false);
        EnableButtons();
        system.SetSelectedGameObject(returnSelected);
    }

    public void SaveSettings() 
    {
        ScoreManager.instance.PlayChime();

        SaveManager.instance.gameData.ySen = yBar.value;
        SaveManager.instance.gameData.xSen = xBar.value;
        SaveManager.instance.gameData.sfxVol = sfxBar.value;
        SaveManager.instance.gameData.musicVol = musicBar.value;
        SaveManager.instance.gameData.menuSfxVol = menuSfxBar.value;
        SaveManager.instance.gameData.menuMusicVol = menuMusicBar.value;

        MusicManager.instance.UpdateMusicVol();
        ScoreManager.instance.UpdateChimeVol();

        CloseSettings();
    }

    #region VolumeMethods

    float CalculatePrecentage(float value)
    {

        float result = Mathf.Floor(value * 100);

        return result;
    }

    public void OnChangeSFX()
    {
        sfxTxt.text = CalculatePrecentage(sfxBar.value).ToString("F0");
    }

    public void OnChangeMusic()
    {
        musicTxt.text = CalculatePrecentage(musicBar.value).ToString("F0");
    }

    public void OnChangeMenuMusic()
    {
        menuMusicTxt.text = CalculatePrecentage(menuMusicBar.value).ToString("F0");
    }

    public void OnChangeMenuSFX()
    {
        menuSfxTxt.text = CalculatePrecentage(menuSfxBar.value).ToString("F0");
    }


    #endregion

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
