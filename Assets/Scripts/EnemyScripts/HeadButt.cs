using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadButt : MonoBehaviour
{

    [SerializeField] int knockBackAmount;
    [SerializeField] bool pull;


    private void OnTriggerEnter(Collider other)
    {
        if (!pull && other.CompareTag("Player"))
        {
            Vector3 dir = (GameManager.instance.player.transform.position - transform.position).normalized * knockBackAmount;
            GameManager.instance.playerScript.pushBack = new Vector3(dir.x, 0, dir.z);
        }
        else
        {
            Vector3 dir = (transform.position - GameManager.instance.player.transform.position).normalized * knockBackAmount;
            GameManager.instance.playerScript.pushBack = new Vector3(dir.x, 0, dir.z);

        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (!pull && other.CompareTag("Player"))
        {
            Vector3 dir = (GameManager.instance.player.transform.position - transform.position).normalized * knockBackAmount;
            GameManager.instance.playerScript.pushBack = new Vector3(-1, 0, -1) * knockBackAmount;
        }
        else
        {
            Vector3 dir = (transform.position - GameManager.instance.player.transform.position).normalized * knockBackAmount;
            GameManager.instance.playerScript.pushBack = new Vector3(1, 0, 1) * knockBackAmount;

        }
    }
}
