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

    bool isUnlocked = false;

    private void Start()
    {
        lockIndicator.color = Color.red;
        doorCounter.color = Color.red;
        UpdateDoorCounter();
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
            doorCounter.text = "Open";
            doorCounter.color = Color.green;
            lockIndicator.color = Color.green;
            isUnlocked = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {

        if (Input.GetButtonDown("Action"))
        {
            if (isUnlocked)
                gameObject.SetActive(false);
        }
    }


}
