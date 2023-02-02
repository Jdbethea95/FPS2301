using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ScorePanel : MonoBehaviour
{
    bool isOpen = false;
    [SerializeField] GameObject selected;
    [SerializeField] EventSystem system;


    public void OpenScorePanel() 
    {
        isOpen = !isOpen;

        if (!isOpen) 
        {
            system.SetSelectedGameObject(selected);
        }
            gameObject.SetActive(isOpen);
    }

}
