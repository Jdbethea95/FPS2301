using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public GameData gameData;

    [Header("----- Setting Saves -----")]
    [SerializeField] float playerXSen;
    [SerializeField] float playerYSen;

    BinaryFormatter formatter = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        NewGameData();
    }

    public void NewGame()
    {
        NewGameData();
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/SaveData.sgo";

        if (File.Exists(filePath) && formatter == null)
        {

            formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            gameData = new GameData(formatter.Deserialize(stream) as GameData);

            if (gameData.topLevel == null && gameData.playerName == "JZH")
                NewGameData();

            Debug.Log(gameData.playerName);
            Debug.Log(gameData.topLevel);

            stream.Close();
            formatter = null;
        }
        else 
        {
            NewGameData();
        }
    }

    public void SaveGame()
    {
        if (formatter == null)
        {
            formatter = new BinaryFormatter();
            string filePath = Application.persistentDataPath + "/SaveData.sgo";

            FileStream stream = new FileStream(filePath, FileMode.Create);

            if (gameData.topLevel == null && gameData.playerName == "JZH")
                NewGameData();

            formatter.Serialize(stream, gameData);
            stream.Close();
            formatter = null; 
        }
    }

    void PlayerPrefCheck()
    {
        if (!PlayerPrefs.HasKey("xSen"))
            PlayerPrefs.SetFloat("xSen", 300f);

        if (!PlayerPrefs.HasKey("ySen"))
            PlayerPrefs.SetFloat("ySen", 300f);

        if (!PlayerPrefs.HasKey("sfxVol"))
            PlayerPrefs.SetFloat("sfxVol", .4f);

        if (!PlayerPrefs.HasKey("musicVol"))
            PlayerPrefs.SetFloat("musicVol", .3f);

        if (!PlayerPrefs.HasKey("menuMusicVol"))
            PlayerPrefs.SetFloat("menuMusicVol", .25f);

        if (!PlayerPrefs.HasKey("menuSfxVol"))
            PlayerPrefs.SetFloat("menuSfxVol", .3f);

    }

    void NewGameData() 
    {

        PlayerPrefCheck();

        gameData = new GameData();
        gameData.xSen = PlayerPrefs.GetFloat("xSen");
        gameData.ySen = PlayerPrefs.GetFloat("ySen");
        gameData.sfxVol = PlayerPrefs.GetFloat("sfxVol");
        gameData.musicVol = PlayerPrefs.GetFloat("musicVol"); ;
        gameData.menuMusicVol = PlayerPrefs.GetFloat("menuMusicVol");
        gameData.menuSfxVol = PlayerPrefs.GetFloat("menuSfxVol");
    }

    private void OnApplicationQuit()
    {
        gameData.xSen = PlayerPrefs.GetFloat("xSen");
        gameData.ySen = PlayerPrefs.GetFloat("ySen");
        gameData.sfxVol = PlayerPrefs.GetFloat("sfxVol");
        gameData.musicVol = PlayerPrefs.GetFloat("musicVol"); ;
        gameData.menuMusicVol = PlayerPrefs.GetFloat("menuMusicVol");
        gameData.menuSfxVol = PlayerPrefs.GetFloat("menuSfxVol");
    }
}
