using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Tooltip("Negative amounts to heal positive damage")]
    [Range(-100, -10)][SerializeField] int heal = -10;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.playerScript.CanHeal)
        {
            GameManager.instance.playerScript.TakeDamage(heal);
            Destroy(gameObject);
        }

    }
}
