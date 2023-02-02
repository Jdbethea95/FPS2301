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

    [Header("-----bComponents -----")]
    [SerializeField] AudioSource audioPlayer;

    [Header("----- Game Goal -----")]
    public int enemyCount;

    [Header("----- UI Menus-----")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject deathMenu;
    public GameObject settingsMenu;
    public GameObject timerUI;

    [Header("----- Settings Components -----")]
    public Slider xSenSlider;
    public Slider ySenSlider;

    [Header("----- Canvas Items-----")]
    [SerializeField] TextMeshProUGUI enemyCountTxt;
    [SerializeField] TextMeshProUGUI timerTxt;
    public Image playerHpBar;
    public Image overHeatBar;
    public Image[] boostBars;

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
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] TextMeshProUGUI PerkScoreTxt;
    [SerializeField] TextMeshProUGUI perkFoundTxt;
    public Score currentScore = new Score();

    [Header("-----Pick-Up Audio -----")]
    [Tooltip("0-speed 1-hp, 2-perk")]
    [SerializeField] AudioClip[] pickUpClips;
    [Range(0f, 1f)][SerializeField] float pickupVol;

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
    public bool onTheClock = true;
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
    }

    private void Start()
    {
        if (levelLocks.Count == 0)
        {
            //Debug.Log("This happened");
            for (int i = 0; i < SaveManager.instance.gameData.levelLocks.Count; i++)
            {
                levelLocks.Add(SaveManager.instance.gameData.levelLocks[i]);
            }
        }

        onTheClock = true;
        audioPlayer.volume = pickupVol;
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
        if (activeMenu != null)
        {
            activeMenu.SetActive(false); 
        }
        activeMenu = null;
    }
    //test Level Locks
    public void WinCondition()
    {

        PauseGame();
        activeMenu = winMenu;
        UpdateScoreCard();

        if (ScoreManager.instance != null && ScoreManager.instance.boards.ContainsKey(SceneManager.GetActiveScene().name))
        {
            ScoreManager.instance.boards[SceneManager.GetActiveScene().name].AddScore(currentScore);
            ScoreManager.instance.Save(ref SaveManager.instance.gameData);

            if (SaveManager.instance.gameData.topLevel == SceneManager.GetActiveScene().name)
            {
                SaveManager.instance.gameData.levelIndx++;

                if (SaveManager.instance.gameData.levelIndx < ScoreManager.instance.Scenes.Count)
                {
                    SaveManager.instance.gameData.topLevel = ScoreManager.instance.Scenes[SaveManager.instance.gameData.levelIndx];
                    SaveManager.instance.gameData.levelLocks.Add(true);
                }
                    
            }
        }

        if (playerScript.PerkFound)
        {
            ScoreManager.instance.ownedList.Add(playerScript.PerkID);
            perkFoundTxt.text = $"Perk {playerScript.PerkName} Found!";
            ScoreManager.instance.Save(ref SaveManager.instance.gameData);
        }
        else { perkFoundTxt.text = ""; }


        playerScript.DeActivatePerks();
        activeMenu.SetActive(true);

    }

    //Implement player name here
    void UpdateScoreCard()
    {
        currentScore.HealthScore = playerScript.CurrentHealth;
        currentScore.SetTimeScore(timerMinutes, timerSeconds);
        currentScore.PlayerName = ScoreManager.instance.playerName;

        timeScoreTxt.text = currentScore.TimeScore.ToString("F0");
        enemyScoreTxt.text = currentScore.EnemyScore.ToString("F0");
        healthScoreTxt.text = currentScore.HealthScore.ToString("F0");
        boostScoreTxt.text = currentScore.BoostScore.ToString("F0");
        totalScoreTxt.text = currentScore.TotalScore.ToString("F0");
        nameTxt.text = currentScore.PlayerName;
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
        if (Time.time > currentTime + timeInterval && onTheClock)
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

    public void ResetTime() 
    {
        timerSeconds = 0;
        timerMinutes = 0;
        StopWatch();
    }

    void GetDoorScripts()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            doorScripts.Add(doors[i].GetComponent<DoorScript>());
        }
    }

    public void ReduceBoost() 
    {
        for (int i = 0; i < boostBars.Length; i++)
        {
            if (boostBars[i].enabled)
            {
                boostBars[i].enabled = false;
                break;
            }
        }
    }

    public void GainBoost() 
    {
        for (int i = boostBars.Length - 1; i >= 0; i--)
        {
            if (!boostBars[i].enabled)
            {
                boostBars[i].enabled = true;
                break;
            }
        }
    }

    public void PlayPickup(int id) 
    {        
        audioPlayer.PlayOneShot(pickUpClips[id]);
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
