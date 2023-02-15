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

        gameData = new GameData();
    }

    public void NewGame()
    {
        gameData = new GameData();
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
                gameData = new GameData();

            Debug.Log(gameData.playerName);
            Debug.Log(gameData.topLevel);

            stream.Close();
            formatter = null;
        }
        else 
        {
            gameData = new GameData();
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
                gameData = new GameData();

            formatter.Serialize(stream, gameData);
            stream.Close();
            formatter = null; 
        }
    }

}
