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
    [SerializeField] GameObject savePlate;
    [SerializeField] GameObject loadPlate;
    [SerializeField] GameObject loadPanel;

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

    [SerializeField] TextMeshProUGUI loadPlayerTape;

    [Header("----- buttons -----")]
    [SerializeField] Button play;
    [SerializeField] Button load;
    [SerializeField] Button save;
    [SerializeField] Button quit;
    [SerializeField] Button scoreBoard;
    [SerializeField] Button settingsBtn;
    [SerializeField] Button credits;
   
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
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
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

        PlayerPrefChange();

        SaveManager.instance.gameData.ySen = yBar.value;
        SaveManager.instance.gameData.xSen = xBar.value;
        SaveManager.instance.gameData.sfxVol = sfxBar.value;
        SaveManager.instance.gameData.musicVol = musicBar.value;
        SaveManager.instance.gameData.menuSfxVol = menuSfxBar.value;
        SaveManager.instance.gameData.menuMusicVol = menuMusicBar.value;

        MusicManager.instance.UpdateMusicVol();
        ScoreManager.instance.UpdateChimeVol();
        MusicManager.instance.UpdateEnemySFX(SaveManager.instance.gameData.sfxVol);
        CloseSettings();
    }

    void PlayerPrefChange() 
    {
        PlayerPrefs.SetFloat("xSen", xBar.value);
        PlayerPrefs.SetFloat("ySen", yBar.value);
        PlayerPrefs.SetFloat("sfxVol", sfxBar.value);
        PlayerPrefs.SetFloat("musicVol", musicBar.value); ;
        PlayerPrefs.SetFloat("menuMusicVol", menuSfxBar.value);
        PlayerPrefs.SetFloat("menuSfxVol", menuMusicBar.value);
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


    public void OpenSavePlate() 
    {
        savePlate.SetActive(true);
        DisableButtons();
    }

    public void OpenLoadPlate() 
    {

        StartCoroutine(LoadPanel());
    }

    public void CloseSavePlate() 
    {
        savePlate.SetActive(false);
        EnableButtons();
    }

    public void CloseLoadPlate() 
    {
        loadPlate.SetActive(false);
        EnableButtons();
    }

    IEnumerator LoadPanel() 
    {
        DisableButtons();
        loadPanel.SetActive(true);
        yield return new WaitForSeconds(1.3f);
        loadPlayerTape.text = SaveManager.instance.gameData.playerName;
        loadPanel.SetActive(false);
        loadPlate.SetActive(true);
    }

    public void DisableButtons() 
    {
        play.enabled = false;
        load.enabled = false;
        save.enabled = false;
        quit.enabled = false;
        credits.enabled = false;
        scoreBoard.enabled = false;
        settingsBtn.enabled = false;
    }

    public void EnableButtons() 
    {
        play.enabled = true;
        load.enabled = true;
        save.enabled = true;
        quit.enabled = true;
        credits.enabled = true;
        scoreBoard.enabled = true;
        settingsBtn.enabled = true;
    }
}
