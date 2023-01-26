using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject namePlate;


    public void CallPlate()
    {
        if (ScoreManager.instance.playerName == "JZH" || ScoreManager.instance.playerName == "")
            namePlate.SetActive(true);
        else
            SceneManager.LoadScene("Lobby");
    }
}
