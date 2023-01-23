using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkPickup : MonoBehaviour
{
    [SerializeField] SO_Perk perk;


    private void Start()
    {
        for (int i = 0; i < ScoreManager.instance.ownedList.Count; i++)
        {
            if (ScoreManager.instance.ownedList[i] == perk.ID)
            {
                Destroy(gameObject);
                break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.PerkPickup(perk);
            Destroy(gameObject);
        }
    }
}
