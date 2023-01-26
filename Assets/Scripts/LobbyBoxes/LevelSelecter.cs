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

            if (levelName.text == "The Hallway")
            {
                isOpen = true;
                portal.material = green;
            }
                

        }

    }

    public bool IncrementIndex(int amount) 
    {

        if (ScoreManager.instance.Scenes.Count > index && amount > 0)
        {            
            index += amount;

            if (index >= ScoreManager.instance.Scenes.Count)
                index = 0;

            return true;
        }
        else if (amount < 0)
        {
            index += amount;

            if (index < 0)
                index = ScoreManager.instance.Scenes.Count - 1;

            return true;
        }

        return false;
    }



}
