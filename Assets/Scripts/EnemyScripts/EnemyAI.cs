using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    [Range(1,100)][SerializeField] int hp = 10;
    [SerializeField] int rotSpeed;

    [Header("----- Gun Stats -----")]
    bool isShooting = false;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [Range(5, 100)] [SerializeField] int shootDist;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [Range(15, 50)] [SerializeField] int bulletSpeed;


    Vector3 playerDir;
    bool playerInRange = false;

    

    private void Start()
    {
        
    }

    private void Update()
    {
        //Checks to see if Player Triggers Sphere Collider
        if (playerInRange)
        {
            //Provides player's direction from enemy
            playerDir = GameManager.instance.player.transform.position - headPos.position;

            //sets destination for enemy pathing
            agent.SetDestination(GameManager.instance.player.transform.position);

            if (agent.remainingDistance < agent.stoppingDistance)
                FacePlayer();

            if (!isShooting)
                StartCoroutine(Shoot()); 
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dmg">Applies Damage to Enemy</param>
    public void TakeDamage(int dmg)
    {

        hp -= dmg;
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (hp <= 0)
            Destroy(gameObject);
    }

    //Adjusts the enemy's rotation upon reaching player
    void FacePlayer() 
    {
        playerDir.y = 0;

        Quaternion rotate = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotate, Time.deltaTime * rotSpeed);
    }

    IEnumerator Shoot() 
    {
        isShooting = true;

        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //Checks for player inside collider trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    //Checks for player exit from collider trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
