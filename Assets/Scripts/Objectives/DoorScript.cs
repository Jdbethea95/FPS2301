using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorScript : MonoBehaviour
{

    [SerializeField] int enemyCount;
    [SerializeField] TextMeshProUGUI doorCounter;
    [SerializeField] Image lockIndicator;

    [Header("----- movement -----")]
    [SerializeField] float dropSpeed;
    [SerializeField] float dropDistance;
    Vector3 drop;

    [Header("----- audio -----")]
    [SerializeField] AudioSource audPlayer;
    [SerializeField] AudioClip[] doorSounds;
    [SerializeField] float doorVol;

    bool isUnlocked = false;
    bool isPlayer = false;
    bool played = false;

    private void Start()
    {
        lockIndicator.color = Color.red;
        doorCounter.color = Color.red;
        UpdateDoorCounter();
        doorVol = SaveManager.instance.gameData.sfxVol;
        drop = new Vector3 (transform.position.x,transform.position.y - dropDistance, transform.position.z);
    }


    private void Update()
    {
        if (isUnlocked && isPlayer)
        {
            OpenDoor();
        }
    }

    public void UpdateDoorCounter(int amount = 0)
    {
        //enemycount gives amounts in negatives when enemy dies. So addition is subtraction
        if (enemyCount + amount > 0)
        {
            enemyCount += amount;
            doorCounter.text = (enemyCount).ToString("F0");
        }
        else
        {
            audPlayer.PlayOneShot(doorSounds[0], doorVol);
            doorCounter.text = "00";
            doorCounter.color = Color.green;
            lockIndicator.color = Color.green;
            isUnlocked = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isUnlocked && other.CompareTag("Player"))
            isPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isUnlocked && other.CompareTag("Player"))
            isPlayer = false;
    }

    void OpenDoor() 
    {
        if (!audPlayer.isPlaying && !played)
        {
            played = true;
            audPlayer.PlayOneShot(doorSounds[1], doorVol);
        }
            

        transform.position = Vector3.Lerp(transform.position, drop, dropSpeed * Time.deltaTime);

        if (transform.position.y <= drop.y + 0.5f)
            gameObject.SetActive(false);
    }

}
