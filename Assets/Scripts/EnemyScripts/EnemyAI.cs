using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;

    [Header("----- Enemy Stats -----")]
    [Range(1, 100)] [SerializeField] int hp = 10;
    [SerializeField] int rotSpeed;
    [SerializeField] int speedMult;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;

    [Header("----- Gun Stats -----")]
    bool isShooting = false;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [Range(5, 100)] [SerializeField] int shootDist;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [Range(15, 50)] [SerializeField] int bulletSpeed;


    Vector3 playerDir;
    bool playerInRange = false;
    float angleToPlayer;


    private void Start()
    {
        GameManager.instance.UpdateEnemiesRemaining(1);
        agent.speed = agent.speed * speedMult;

        if (agent.speed == 0)
            agent.stoppingDistance = int.MaxValue;
    }

    private void Update()
    {
        // we don't have a shoot anim so we need to talk about that 
        //animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
        //Checks to see if Player Triggers Sphere Collider
        if (playerInRange)
        {
            canSeePlayer();
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
        {
            GameManager.instance.UpdateEnemiesRemaining(-1);
            Destroy(gameObject);
        }

    }
    void canSeePlayer()
    {
        //Provides player's direction from enemy
        playerDir = GameManager.instance.player.transform.position - headPos.position;

        float pdy = playerDir.y;
        playerDir.y = 0;

        //AI's veiw section
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        playerDir.y = pdy;

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
        
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                //sets destination for enemy pathing
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                    FacePlayer();

                //angleToPlayer <= shootAngle  &&
                if (!isShooting && shootDist >= Vector3.Distance(transform.position, GameManager.instance.player.transform.position))
                    StartCoroutine(Shoot());
            }
        }
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

        animator.SetTrigger("Shoot");

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
