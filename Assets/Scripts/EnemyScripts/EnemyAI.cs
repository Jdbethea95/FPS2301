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
    [SerializeField] Collider body;

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
    bool isDead = false;


    private void Start()
    {
        GameManager.instance.UpdateEnemiesRemaining(1);
        agent.speed = agent.speed * speedMult;

        if (agent.speed == 0)
            agent.stoppingDistance = int.MaxValue;
    }

    private void Update()
    {
        
        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        //Checks to see if Player Triggers Sphere Collider
        if (playerInRange && !isDead)
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
        animator.SetBool("PlayerNear", true);
        animator.SetTrigger("Hurt");
        agent.SetDestination(GameManager.instance.player.transform.position);

        if (hp <= 0)
        {
            animator.SetBool("Dead", true);
            isDead = true;
            body.enabled = false;
            GameManager.instance.UpdateEnemiesRemaining(-1);
            GameManager.instance.currentScore.EnemyScore = 1;
            //Destroy(gameObject);
        }

    }
    void canSeePlayer()
    {
        //Provides player's direction from enemy
        playerDir = GameManager.instance.player.transform.position - headPos.position;

        //saves directional Y and sets to zero for angle calculations to ignore Y axis
        float pdy = playerDir.y;
        playerDir.y = 0;

        //AI's veiw section
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        //resets Y back into playerDir for Raycast
        playerDir.y = pdy;

        //Debug.Log($"Angle{angleToPlayer} :: Dir{playerDir}");
        //Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
        
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                //sets destination for enemy pathing
                agent.SetDestination(GameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                    FacePlayer();

                
                if (!isShooting && angleToPlayer <= shootAngle)
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
        int offest = Random.Range(0, 2);

        Vector3 accuracy = new Vector3(GameManager.instance.playerScript.COM.x + offest,
                                       GameManager.instance.playerScript.COM.y,
                                       GameManager.instance.playerScript.COM.z + offest);

        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (accuracy - transform.position).normalized * bulletSpeed;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    //Checks for player inside collider trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            animator.SetBool("PlayerNear", true);
        }
    }

    //Checks for player exit from collider trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            animator.SetBool("PlayerNear", false);
        }
    }

}
