using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(5, 20)] [SerializeField] int bulletDMG;

    [SerializeField] int timer;

    private void Start()
    {
        //Destroys bullet after alloted amount of time in for collision happens
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        //damages player from collision of bullet
        if (other.CompareTag("Player"))
            GameManager.instance.playerScript.TakeDamage(bulletDMG);

        //destroys bullet object upon collision with any object not on enemy layer
        Destroy(gameObject);
    }
}
