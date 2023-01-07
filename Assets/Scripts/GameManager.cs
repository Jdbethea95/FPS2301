using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Singlton Script used for game managment and player calls
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public PlayerController playerScript;

    int enemyCount;

    //aquires player gameobject and script, Remember awake happens before Start.
    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    public void UpdateEnemiesRemaining(int amount) 
    {
        enemyCount += amount;

        if (enemyCount <= 0) 
        {
            Debug.Log("All Enemies Eliminated!!");
        }
    }

}
