using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour, ISaveData
{
    public static ScoreManager instance;
    public Dictionary<string, LevelScores> boards = new Dictionary<string, LevelScores>();
    public List<string> Scenes;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        foreach (string name in Scenes)
        {
            boards.Add(name, new LevelScores(name));
        }

    }

    public void Load(GameData data)
    {
        int i = 0;
        foreach (var item in boards)
        {
            boards[item.Key].ScoreBoard.Clear();
            if (i < data.scores.Count)
            {
                for (int x = 0; x < data.scores[i].ScoreBoard.Count; x++)
                {
                    boards[item.Key].ScoreBoard.Add(data.scores[i].ScoreBoard[x]);
                }
                i++;
            }
            
        }
    }

    public void Save(ref GameData data)
    {
        data.scores.Clear();

        foreach (var item in boards)
        {
            data.scores.Add(item.Value);
        }
    }

}
