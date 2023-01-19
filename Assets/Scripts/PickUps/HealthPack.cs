using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Tooltip("Negative amounts to heal positive damage")]
    [Range(10, 50)][SerializeField] int heal = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.playerScript.CanHeal)
        {
            GameManager.instance.playerScript.HealPlayer(heal);
            Destroy(gameObject);
        }

    }
}
