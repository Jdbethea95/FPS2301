using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{

    public void Resume()
    {
        ScoreManager.instance.PlayChime();
        GameManager.instance.UnPause();
        GameManager.instance.isPaused = !GameManager.instance.isPaused;
    }

    public void Restart()
    {
        ScoreManager.instance.PlayChime();
        GameManager.instance.playerScript.DeActivatePerks();
        GameManager.instance.UnPause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        ScoreManager.instance.PlayChime();
        GameManager.instance.playerScript.DeActivatePerks();
        Application.Quit();
    }

    public void MenuQuit()
    {
        ScoreManager.instance.PlayChime();
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        ScoreManager.instance.PlayChime();

        GameManager.instance.playerScript.DeActivatePerks();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameManager.instance.LoadingScene("MainMenu");
    }

    public void CreditToMenu() 
    {
        ScoreManager.instance.PlayChime();

        SceneManager.LoadScene("MainMenu");
    }

    public void MenutoCredit() 
    {
        ScoreManager.instance.PlayChime();

        SceneManager.LoadScene("CreditScene");
    }

    public void TempPlay()
    {
        ScoreManager.instance.PlayChime();
    }

    public void Lobby()
    {
        ScoreManager.instance.PlayChime();

        GameManager.instance.LoadingScene("Lobby");
        GameManager.instance.UnPause();
    }

    public void SaveButton() 
    {
        ScoreManager.instance.PlayChime();

        SaveManager.instance.SaveGame();
    }

    public void LoadButton() 
    {
        ScoreManager.instance.PlayChime();

        SaveManager.instance.LoadGame();
        ScoreManager.instance.Load(SaveManager.instance.gameData);
    }

    #region SettingsMethods

    public void Settings()
    {
        ScoreManager.instance.PlayChime();

        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = GameManager.instance.settingsMenu;
        GameManager.instance.activeMenu.SetActive(true);

        GameManager.instance.system.SetSelectedGameObject(GameManager.instance.settingSelected);

        //update sensitivity slider position with current value
        #region ValueUpdate
        GameManager.instance.xSenSlider.value = GameManager.instance.playerScript.cam.XSen;
        GameManager.instance.ySenSlider.value = GameManager.instance.playerScript.cam.YSen;
        GameManager.instance.sfxBar.value = SaveManager.instance.gameData.sfxVol;
        GameManager.instance.musicBar.value = SaveManager.instance.gameData.musicVol;
        GameManager.instance.menuSfxBar.value = SaveManager.instance.gameData.menuSfxVol;
        GameManager.instance.menuMusicBar.value = SaveManager.instance.gameData.menuMusicVol;
        #endregion
    }

    public void SettingsBack()
    {
        ScoreManager.instance.PlayChime();

        GameManager.instance.activeMenu.SetActive(false);
        GameManager.instance.activeMenu = GameManager.instance.pauseMenu;
        GameManager.instance.activeMenu.SetActive(true);

        GameManager.instance.system.SetSelectedGameObject(GameManager.instance.pauseSelected);
    }

    public void SettingsSave()
    {
        ScoreManager.instance.PlayChime();

        GameManager.instance.playerScript.cam.XSen = GameManager.instance.xSenSlider.value;
        GameManager.instance.playerScript.cam.YSen = GameManager.instance.ySenSlider.value;

        SaveManager.instance.gameData.xSen = GameManager.instance.playerScript.cam.XSen;
        SaveManager.instance.gameData.ySen = GameManager.instance.playerScript.cam.YSen;
        SaveManager.instance.gameData.sfxVol = GameManager.instance.sfxBar.value;
        SaveManager.instance.gameData.musicVol = GameManager.instance.musicBar.value;
        SaveManager.instance.gameData.menuSfxVol = GameManager.instance.menuSfxBar.value;
        SaveManager.instance.gameData.menuMusicVol = GameManager.instance.menuMusicBar.value;

        MusicManager.instance.UpdateMusicVol();
        MusicManager.instance.UpdateEnemySFX(GameManager.instance.sfxBar.value);
        ScoreManager.instance.UpdateChimeVol();
        GameManager.instance.playerScript.UpdatePlayerSFX();
        GameManager.instance.UpdatePickUpVol();

        SettingsBack();
    }

    float CalculatePrecentage(float value)
    {

        float result = Mathf.Floor(value * 100);

        return result;
    }

    public void OnChangeSFX()
    {
        GameManager.instance.sfxTxt.text = CalculatePrecentage(GameManager.instance.sfxBar.value).ToString("F0");
    }

    public void OnChangeMusic()
    {
        GameManager.instance.musicTxt.text = CalculatePrecentage(GameManager.instance.musicBar.value).ToString("F0");
    }

    public void OnChangeMenuMusic()
    {
        GameManager.instance.menuMusicTxt.text = CalculatePrecentage(GameManager.instance.menuMusicBar.value).ToString("F0");
    }

    public void OnChangeMenuSFX()
    {
        GameManager.instance.menuSfxTxt.text = CalculatePrecentage(GameManager.instance.menuSfxBar.value).ToString("F0");
    } 
    #endregion

}
