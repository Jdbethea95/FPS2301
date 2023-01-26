using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    int totalScore, _healthBonus, _boostBonus, _enemyBonus, _timeBonus = 18590;
    string _playerName;

    public int HealthScore
    {
        get { return _healthBonus; }
        set
        {
            _healthBonus = Mathf.FloorToInt(value * 0.1f) * 10;
        }
    }

    public int BoostScore
    {
        get { return _boostBonus; }
        set { _boostBonus += value * 5; }
    }

    public int EnemyScore
    {
        get { return _enemyBonus; }
        set { _enemyBonus += value * 100; }
    }

    public int TimeScore
    {
        get { return _timeBonus; }
        private set { }
    }

    public int TotalScore
    {
        get { return (_timeBonus + _enemyBonus + _healthBonus + _boostBonus); }
        private set { }
    }

    public string PlayerName 
    {
        get { return _playerName; }
        set 
        {
            if (value.Length > 4)
                _playerName = "NAH";
            else
                _playerName = value.ToUpper();
        }
    }

    public Score() 
    {
        HealthScore = 0;
        BoostScore = 0;
        EnemyScore = 0;
    }

    /// <summary>
    /// Reduces base score 18590 by 600 per minute, and 
    /// by 10 per Second
    /// </summary>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    public void SetTimeScore(int minutes, int seconds)
    {
        _timeBonus -= (minutes * 600) + (seconds * 10);
    }




}
