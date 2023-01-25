using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] bool isEntrance;
    [SerializeField] bool isStart;

    bool _isTriggered = false;

    public bool Triggered 
    {
        get { return _isTriggered; }
    }

    public void ResetTrigger() 
    {
        _isTriggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (isEntrance && !GameManager.instance.timerUI.activeSelf)
            {
                GameManager.instance.ResetTime();
                GameManager.instance.onTheClock = false;
                GameManager.instance.timerUI.SetActive(true);
            }
            else if (isEntrance)
            {
                GameManager.instance.timerUI.SetActive(false);
            }else
                _isTriggered = true;

            if (isStart)
            {
                GameManager.instance.ResetTime();
                GameManager.instance.onTheClock = true;
            }
                


        }
            
    }
}
