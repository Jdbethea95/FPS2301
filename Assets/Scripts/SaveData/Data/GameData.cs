using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<LevelScores> scores = new List<LevelScores>();
    public List<string> perkIds = new List<string>();
    public List<bool> levelLocks = new List<bool>();

    public string playerName;

    public float xSen;
    public float ySen;

    public string topLevel;
    public int levelIndx;

    public GameData()
    {
        xSen = 300f;
        ySen = 300f;
        levelLocks.Add(true);
        topLevel = "The Hallway";
        levelIndx = 0;
        playerName = "JZH";
    }

    public GameData(GameData data)
    {
        UpdateScores(data.scores);
        UpdatePerks(data.perkIds);
        UpdateLevels(data.levelLocks);
        xSen = data.xSen;
        ySen = data.ySen;
        playerName = data.playerName;
    }


    public void UpdateScores(List<LevelScores> _sc)
    {
        for (int i = 0; i < _sc.Count; i++)
        {
            LevelScores currScore = new LevelScores();
            for (int x = 0; x < _sc[i].ScoreBoard.Count; x++)
            {
                Debug.Log($"{_sc[i].ScoreBoard[x].TotalScore}");
                currScore.ScoreBoard.Add(_sc[i].ScoreBoard[x]);
            }
            scores.Add(currScore);
        }
    }

    public void UpdatePerks(List<string> _str)
    {
        for (int i = 0; i < _str.Count; i++)
        {
            perkIds.Add(_str[i]);
        }
    }

    public void UpdateLevels(List<bool> _bl)
    {
        for (int i = 0; i < _bl.Count; i++)
        {
            levelLocks.Add(_bl[i]);
        }
    }

}
