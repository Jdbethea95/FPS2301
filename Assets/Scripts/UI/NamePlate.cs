using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;


public class NamePlate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI input;
    [SerializeField] TextMeshProUGUI warnings;
    [SerializeField] GameObject selected;
    [SerializeField] EventSystem system;

    [SerializeField] MenuScript menu;

    public void EnterButton() 
    {

        if (input.text.Length > 4)
        {
            warnings.color = Color.red;
            input.text = "";
        }
        else 
        {
            ScoreManager.instance.playerName = input.text;
            input.text = "";
            MenuLoad.instance.LoadingScene("Lobby");
        }
    }

    public void ExitButton() 
    {
        system.SetSelectedGameObject(selected);
        warnings.color = Color.white;
        menu.EnableButtons();
        gameObject.SetActive(false);
    }
}
