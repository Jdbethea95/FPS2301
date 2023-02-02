using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardUI : MonoBehaviour
{
    [SerializeField] GameObject row;
    [SerializeField] TMP_Dropdown drop;
    List<GameObject> rows = new List<GameObject>();
    List<LevelScores> levels = new List<LevelScores>();



    private void Start()
    {
        SetUpGrid();
    }

    public void SetUpGrid() 
    {
        

        foreach (var lvl in ScoreManager.instance.boards)
        {
            levels.Add(lvl.Value);
        }

        RemoveRows();
        UpdateGrid(levels[drop.value]);

        levels.Clear();

    }

    void UpdateGrid(LevelScores level)
    {
        for (int i = 0; i < level.ScoreBoard.Count; i++)
        {
            
            rows.Add(Instantiate(row, transform));
            RowUI ui = rows[i].GetComponent<RowUI>();

            //setting rank based on position in list
            switch (i)
            {
                case 0:
                    ui.SetRank($"{i+1}st");
                    break;
                case 1:
                    ui.SetRank($"{i + 1}nd");
                    break;
                case 2:
                    ui.SetRank($"{i + 1}rd");
                    break;
                default:
                    ui.SetRank($"{i + 1}th");
                    break;
            }

            ui.SetScore(level.ScoreBoard[i].TotalScore.ToString("F0"));
            ui.SetName(level.ScoreBoard[i].PlayerName);

        }
    }

    void RemoveRows() 
    {
        if (rows.Count <= 0)
            return;

        for (int i = 0; i < rows.Count; i++)
        {
            Destroy(rows[i]);
        }
        rows.Clear();
    }

}
