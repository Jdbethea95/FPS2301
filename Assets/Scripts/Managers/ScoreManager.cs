using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Dictionary<string, LevelScores> boards = new Dictionary<string, LevelScores>();
    [SerializeField] List<string> Scenes;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (string name in Scenes)
        {
            boards.Add(name, new LevelScores(name));
            Debug.Log(name);
        }

    }


}
