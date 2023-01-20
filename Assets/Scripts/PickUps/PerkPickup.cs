using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkPickup : MonoBehaviour
{
    [SerializeField] SO_Perk perk;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.perkPickup(perk);
            Destroy(gameObject);
        }
    }
}
