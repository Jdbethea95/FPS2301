using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public GameObject settingsMenu;

    [Header("----- Settings Components -----")]
    public Slider xSenSlider;
    public Slider ySenSlider;

    [Header("----- Canvas Items-----")]
    [SerializeField] TextMeshProUGUI enemyCountTxt;
    [SerializeField] TextMeshProUGUI timerTxt;
    public Image playerHpBar;

    [Header("----- Flash Items-----")]
    [SerializeField] GameObject topFlash;
    [SerializeField] GameObject leftFlash;
    [SerializeField] GameObject rightFlash;
    [SerializeField] GameObject bottomFlash;

    [Header("----- Score Items-----")]
    [SerializeField] TextMeshProUGUI totalScoreTxt;
    [SerializeField] TextMeshProUGUI timeScoreTxt;
    [SerializeField] TextMeshProUGUI enemyScoreTxt;
    [SerializeField] TextMeshProUGUI healthScoreTxt;
    [SerializeField] TextMeshProUGUI boostScoreTxt;
    [SerializeField] TextMeshProUGUI PerkScoreTxt;
    public Score currentScore = new Score();

    [SerializeField] List<GameObject> doors;
    [SerializeField] List<DoorScript> doorScripts;

    //timer variables
    int timerSeconds;
    int timerMinutes;
    //Time.time Trackers
    int timeInterval = 1;
    float currentTime = 0f;

    //Pause State Variables
    public bool isPaused;
    float timeScaleOrig;

    public List<bool> levelLocks;
    //aquires player gameobject and script, Remember awake happens before Start.
    private void Awake()
    {
        instance = this;

        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        //I really hate this
        doors.AddRange(GameObject.FindGameObjectsWithTag("Door"));
        GetDoorScripts();

        timeScaleOrig = Time.timeScale;
        levelLocks.Add(true);
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

        StopWatch();

    }

    #region MenuConditions

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

    public void WinCondition()
    {

        PauseGame();
        activeMenu = winMenu;
        UpdateScoreCard();

        if (ScoreManager.instance != null && ScoreManager.instance.boards.ContainsKey(SceneManager.GetActiveScene().name))
        {
            ScoreManager.instance.boards[SceneManager.GetActiveScene().name].AddScore(currentScore);
            ScoreManager.instance.Save(ref SaveManager.instance.gameData);
        }


        activeMenu.SetActive(true);

    }

    //Implement player name here
    void UpdateScoreCard()
    {
        currentScore.HealthScore = playerScript.CurrentHealth;
        currentScore.SetTimeScore(timerMinutes, timerSeconds);

        timeScoreTxt.text = currentScore.TimeScore.ToString("F0");
        enemyScoreTxt.text = currentScore.EnemyScore.ToString("F0");
        healthScoreTxt.text = currentScore.HealthScore.ToString("F0");
        boostScoreTxt.text = currentScore.BoostScore.ToString("F0");
        totalScoreTxt.text = currentScore.TotalScore.ToString("F0");
    }

    public void PlayerDeath()
    {
        PauseGame();
        activeMenu = deathMenu;
        activeMenu.SetActive(true);
    }

    #endregion

    public void UpdateEnemiesRemaining(int amount)
    {
        enemyCount += amount;
        enemyCountTxt.text = enemyCount.ToString("F0");

        if (amount < 0)
        {
            for (int i = 0; i < doors.Count; i++)
            {
                doorScripts[i].UpdateDoorCounter(amount);
            }
        }
    }

    void StopWatch()
    {
        if (Time.time > currentTime + timeInterval)
        {
            timerSeconds += timeInterval;

            if (timerSeconds >= 60)
            {
                timerMinutes++;
                timerSeconds = 0;
            }

            currentTime = Time.time;
            //minute conversion
            if (timerMinutes <= 0)
                timerTxt.text = $"00:";
            else if (timerMinutes < 10)
                timerTxt.text = $"0{timerMinutes}:";
            else
                timerTxt.text = $"{timerMinutes}:";

            //second conversion
            if (timerSeconds < 10)
                timerTxt.text += $"0{timerSeconds}";
            else
                timerTxt.text += $"{timerSeconds}";

        }
    }

    void GetDoorScripts()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doorScripts.Add(doors[i].GetComponent<DoorScript>());
        }
    }

    #region Flashes

    public IEnumerator TopFlash()
    {
        topFlash.SetActive(true);
        yield return new WaitForSeconds(.5f);
        topFlash.SetActive(false);
    }

    public IEnumerator LeftFlash()
    {
        leftFlash.SetActive(true);
        yield return new WaitForSeconds(.5f);
        leftFlash.SetActive(false);
    }

    public IEnumerator RightFlash()
    {
        rightFlash.SetActive(true);
        yield return new WaitForSeconds(.5f);
        rightFlash.SetActive(false);
    }

    public IEnumerator BottomFlash()
    {
        bottomFlash.SetActive(true);
        yield return new WaitForSeconds(.5f);
        bottomFlash.SetActive(false);
    }

    #endregion

}
