using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sludge : MonoBehaviour
{
    [SerializeField] int sludgeDamage;
    [SerializeField] int toxicRate;
    int speedSaver;
    bool isSludged = false;
    bool SludgingTime = false;


    private void Update()
    {
        if (!SludgingTime && isSludged) 
        {
            StartCoroutine(ToxicDamage());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSludged = true;
            GameManager.instance.playerScript.StopDashPart();
            GameManager.instance.playerScript.SetSpeed(GameManager.instance.playerScript.BaseSpeed / 2);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isSludged = false;
            GameManager.instance.playerScript.StopDashPart();
            GameManager.instance.playerScript.SetSpeed(GameManager.instance.playerScript.BaseSpeed);
        }
    }

    IEnumerator ToxicDamage() 
    {
        SludgingTime = true;
        GameManager.instance.playerScript.TakeDamage(sludgeDamage, transform.forward);
        GameManager.instance.playerScript.StopDashPart();
        yield return new WaitForSeconds(toxicRate);
        SludgingTime = false;
    }

}
