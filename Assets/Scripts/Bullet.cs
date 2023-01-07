using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(5, 20)] [SerializeField] int bulletDMG;

    [SerializeField] int timer;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            GameManager.instance.playerScript.TakeDamage(bulletDMG);

        Destroy(gameObject);
    }
}
