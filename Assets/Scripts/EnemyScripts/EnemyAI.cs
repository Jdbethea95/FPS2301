using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    enum EnemyType { Shoot, Explode }

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform headPos;
    [SerializeField] Animator animator;
    [SerializeField] Collider body;
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] SphereCollider ranger;
    [SerializeField] BotKiller killer;

    [Header("----- Enemy Stats -----")]
    [Range(1, 100)] [SerializeField] int hp = 10;
    [SerializeField] int rotSpeed;
    [SerializeField] int speedMult;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] int despawnTime;
    [SerializeField] EnemyType type;

    [Header("----- death Stats -----")]
    [SerializeField] int graveDepth;
    [SerializeField] float sinkSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject explosion;
    [SerializeField] Transform shootPos;
    [Range(0, 3)] [SerializeField] int percision;
    [Range(5, 100)] [SerializeField] int shootDist;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [Range(15, 50)] [SerializeField] int bulletSpeed;
    bool isShooting = false;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audEnemyTakesDamage;
    [Range(0, 5)] [SerializeField] float audEnemyTakesDamageVol;
    [SerializeField] AudioClip[] audEnemySteps;
    [Range(0, 5)] [SerializeField] float audEnemyStepsVol;
    [SerializeField] AudioClip[] audEnemyShoot;
    [Range(0, 5)] [SerializeField] float audEnemyShootVol;
    [SerializeField] AudioClip audEnemydeath;

    [Header("----- Side Options -----")]
    public bool reportDeath = true;

    Vector3 playerDir;
    Vector3 deathSpot;
    bool playerInRange = false;
    float angleToPlayer;
    bool isDead = false;
    bool isPlayingSteps;
    float despawnTimer;



    public int ShootDist
    {
        private get { return shootDist; }
        set
        {
            if (value > 5 && value < 100)
                shootDist = value;
        }
    }
    public int SpeedMult
    {
        private get { return speedMult; }
        set { speedMult = value; }
    }

    private void Start()
    {
        agent.speed = agent.speed * speedMult;

        if (agent.speed == 0)
            agent.stoppingDistance = int.MaxValue;

        if (type == EnemyType.Explode)
        {
            shootDist = 2;
            agent.stoppingDistance = 2;
        }

    }

    private void Update()
    {

        animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        //Checks to see if Player Triggers Sphere Collider
        if (playerInRange && !isDead)
        {
            canSeePlayer();
        }
        else if (isDead)
        {
            if (Time.time > despawnTimer + despawnTime)
            {

                //transform.position = Vector3.Lerp(transform.position, deathSpot, sinkSpeed * Time.deltaTime);
                if (killer.killIt)
                    Destroy(gameObject);

                killer.killIt = true;
                despawnTimer = Time.time;


            }
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
        //animator.SetTrigger("Hurt");

        if (hp <= 0)
        {
            body.enabled = false;
            ranger.enabled = false;
            agent.SetDestination(transform.position);
            agent.enabled = false;
            animator.SetBool("Dead", true);
            isDead = true;

            audioPlayer.PlayOneShot(audEnemydeath);

            deathSpot = new Vector3(transform.position.x, transform.position.y - graveDepth,
                                    transform.position.z);

            if (reportDeath)
            {
                GameManager.instance.UpdateEnemiesRemaining(-1);
                GameManager.instance.currentScore.EnemyScore = 1;
            }

            GameManager.instance.playerScript.DashPointGain();
            despawnTimer = Time.time;
            //Destroy(gameObject);
        }
        else
        {

            agent.SetDestination(GameManager.instance.player.transform.position);
            if (agent.remainingDistance > agent.stoppingDistance && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }
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
                if (agent.remainingDistance > agent.stoppingDistance && !isPlayingSteps)
                {
                    StartCoroutine(playSteps());
                }

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    FacePlayer();
                }


                switch (type)
                {
                    case EnemyType.Shoot:
                        if (!isShooting && angleToPlayer <= shootAngle && agent.remainingDistance <= shootDist)
                            StartCoroutine(Shoot());
                        break;
                    case EnemyType.Explode:
                        if (Vector3.Distance(transform.position, GameManager.instance.player.transform.position) <= shootDist)
                            Explode();
                        break;
                }


            }
        }
    }

    IEnumerator playSteps()
    {
        if (agent.isOnNavMesh)
        {

            isPlayingSteps = true;
            audioPlayer.PlayOneShot(audEnemySteps[Random.Range(0, audEnemySteps.Length)], audEnemyStepsVol);

            if (agent.speed == 3)
            {
                yield return new WaitForSeconds(0.4f);
            }
            else if (agent.speed == 6)
            {
                yield return new WaitForSeconds(0.3f);
            }
            isPlayingSteps = false;
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
        int offest = Random.Range(0, percision);

        Vector3 accuracy = new Vector3(GameManager.instance.playerScript.COM.x + offest,
                                       GameManager.instance.playerScript.COM.y,
                                       GameManager.instance.playerScript.COM.z + offest);
        audioPlayer.PlayOneShot(audEnemyShoot[Random.Range(0, audEnemyShoot.Length)], audEnemyShootVol);

        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = (accuracy - transform.position).normalized * bulletSpeed;

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        TakeDamage(int.MaxValue);
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
