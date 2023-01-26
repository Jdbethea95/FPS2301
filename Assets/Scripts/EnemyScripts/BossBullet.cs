using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Range(5, 20)] [SerializeField] int bulletDMG;

    [SerializeField] int timer;
    [SerializeField] float speed;

    [SerializeField] GameObject explosion;

    private void Start()
    {
        //Destroys bullet after alloted amount of time in for collision happens
        Destroy(gameObject, timer);
    }


    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //damages player from collision of bullet
        if (other.CompareTag("Player"))
        {
            Vector3 dir = GameManager.instance.player.transform.position - transform.position;
            GameManager.instance.playerScript.TakeDamage(bulletDMG, dir);
        }


        Instantiate(explosion, transform.position, explosion.transform.rotation);

        //destroys bullet object upon collision with any object not on enemy layer
        Destroy(gameObject);
    }
}
