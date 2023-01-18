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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        string filePath = Application.persistentDataPath + "/SaveData.sgo";

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            gameData = new GameData(formatter.Deserialize(stream) as GameData);

            stream.Close();
        }
    }

    public void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/SaveData.sgo";

        FileStream stream = new FileStream(filePath, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

}
