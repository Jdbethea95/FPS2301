using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.TakeDamage(int.MaxValue, Vector3.zero);
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAI>().TakeDamage(int.MaxValue);
        }
    }

}
