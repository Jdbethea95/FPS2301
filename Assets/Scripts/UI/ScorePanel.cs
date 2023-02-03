using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ScorePanel : MonoBehaviour
{
    bool isOpen = false;
    [SerializeField] GameObject selected;
    [SerializeField] GameObject boardSelected;
    [SerializeField] EventSystem system;
    [SerializeField] MenuScript menu;

    public void OpenScorePanel() 
    {
        isOpen = !isOpen;

        if (!isOpen)
        {
            system.SetSelectedGameObject(selected);
            menu.EnableButtons();
        }
        else
        {
            system.SetSelectedGameObject(boardSelected);
            menu.DisableButtons();
        }
            

            gameObject.SetActive(isOpen);
    }

}
