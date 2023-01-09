using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPack : MonoBehaviour
{
    [Tooltip("The amount player speed is increased by")]
    [Range(1, 10)] [SerializeField] int speedBoost = 5;

    [Tooltip("The amount of time speed boost is active")]
    [Range(1f, 10f)][SerializeField] int duration = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.SpeedBoost(speedBoost, duration);
            Destroy(gameObject);
        }
    }
}
