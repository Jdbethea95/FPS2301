using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelSelecter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] Renderer portal;
    [SerializeField] Material green;
    [SerializeField] Material red;

    [SerializeField] int index = 0;
    public bool isOpen = false;

    public int Index 
    {
        get { return index; }
        private set { index = value; }
    }


    private void Start()
    {
        UpdateLevelText();
    }

    public void UpdateLevelText()
    {
        if (ScoreManager.instance.Scenes.Count > index)
            levelName.text = ScoreManager.instance.Scenes[index];

        Debug.Log(ScoreManager.instance.Scenes[index] + " : " + ScoreManager.instance.Scenes.Count);

        if (GameManager.instance.levelLocks.Count > index)
        {
            if (GameManager.instance.levelLocks[index])
            {
                isOpen = true;
                portal.material = green;
            }
            else
            {
                isOpen = false;
                portal.material = red;
            }
        }
        else 
        {
            isOpen = false;
            portal.material = red;
        }

    }

    public void IncrementIndex(int amount) 
    {
        if (ScoreManager.instance.Scenes.Count > index &&
            GameManager.instance.levelLocks.Count > index && amount > 0)
        {
            index += amount;
        }
        else if (index > 0 && amount < 0)
        {
            index += amount;
        }
    }



}
