using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//Singlton Script used for game managment and player calls
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("----- Player -----")]
    public GameObject player;
    public PlayerController playerScript;

    [Header("----- Game Goal -----")]
    public int enemyCount;

    [Header("----- UI Menus-----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject deathMenu;

    [Header("----- Canvas Items-----")]
    [SerializeField] TextMeshProUGUI enemyCountTxt;

    //Pause State Variables
    public bool isPaused;
    float timeScaleOrig;

    //aquires player gameobject and script, Remember awake happens before Start.
    private void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        timeScaleOrig = Time.timeScale;
    }


    private void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
                PauseGame();

        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void UnPause() 
    {
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void UpdateEnemiesRemaining(int amount) 
    {
        enemyCount += amount;
        enemyCountTxt.text = enemyCount.ToString("F0");

        if (enemyCount <= 0) 
        {
            PauseGame();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
    }

}
