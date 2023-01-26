using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class NamePlate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI input;
    [SerializeField] TextMeshProUGUI warnings;

    public void EnterButton() 
    {
        Debug.Log(input.text.Length);
        if (input.text.Length > 4)
        {
            warnings.color = Color.red;
            input.text = "";
        }
        else 
        {
            ScoreManager.instance.playerName = input.text;
            input.text = "";
            SceneManager.LoadScene("Lobby");
        }
    }

    public void ExitButton() 
    {
        warnings.color = Color.white;
        gameObject.SetActive(false);
    }
}
