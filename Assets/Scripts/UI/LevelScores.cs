using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelScores
{
    public string levelName;

    List<Score> _scoreBoard = new List<Score>();
    public List<Score> ScoreBoard 
    {
        get { return _scoreBoard; }
        private set { }
    }

    public LevelScores() 
    {
        levelName = "";
    }

    public LevelScores(string name) 
    {
        levelName = name;
    }

    /// <summary>
    /// Adds score to the level board if the score was greater than any of the top 10.
    /// </summary>
    /// <param name="score"></param>
    /// <returns>Returns true if score is added, false if it wasn't</returns>
    public bool AddScore(Score score) 
    {

        if (_scoreBoard.Count == 0)
        {
            _scoreBoard.Add(score);
            return true;
        }


        for (int i = 0; i < _scoreBoard.Count; i++)
        {
            if (ScoreBoard[i].TotalScore < score.TotalScore)
            {
                ScoreBoard.Insert(i, score);

                if (ScoreBoard.Count > 10)
                    ScoreBoard.RemoveAt(ScoreBoard.Count - 1);
                
                return true;
            }
        }

        if (ScoreBoard.Count < 10)
            ScoreBoard.Add(score);

        return false;
    }

}
