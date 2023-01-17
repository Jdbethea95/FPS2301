using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RowUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _rank;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _playerName;


    public void SetRank(string rank) => _rank.text = rank;

    public void SetScore(string score) => _score.text = score;

    public void SetName(string name) => _playerName.text = name;
}
