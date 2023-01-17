using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    bool isOpen = false;

    public void OpenScorePanel() 
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);
    }

}
