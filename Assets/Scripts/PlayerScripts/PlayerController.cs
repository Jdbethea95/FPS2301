using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] ParticleSystem dashParticles;
    public CameraController cam;
    [SerializeField] GameObject com;
    [SerializeField] AudioSource audioPlayer;

    [Header("----- Player Stats -----")]
    [Range(10, 100)] [SerializeField] int hp = 50;
    int maxHp = 50;
    [Range(1, 20)] [SerializeField] int speed;
    [Range(1, 5)] [SerializeField] int reduceRate = 2;
    int baseSpeed;
    float speedTimer;

    [Header("----- Jump Stats -----")]
    [SerializeField] float gravity = 9.8f;
    [Range(5, 10)] [SerializeField] int jumpHeight;
    [Range(1, 3)] [SerializeField] int jumpMax;
    [SerializeField] int jumpCount;

    [Header("----- Gun Stats -----")]
    [Range(5, 50)] [SerializeField] int shootDamage = 10;
    [Range(15, 200)] [SerializeField] int shootDist;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [SerializeField] ParticleSystem gunFlash;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audPlayerTakesDamage;
    [Range(0, 1)][SerializeField] float audPlayerTakesDamageVol;
    [SerializeField] AudioClip[] audPlayerJump;
    [Range(0, 1)][SerializeField] float audPlayerJumpVol;
    [SerializeField] AudioClip[] audPlayerSteps;
    [Range(0, 1)][SerializeField] float audPlayerStepsVol;

    bool isShooting = false;
    bool isPlayingSteps;

    Vector3 move;
    Vector3 velocity;
    public Vector3 hitDirection = Vector3.zero;

    #region Properties

    public bool CanHeal
    {
        get { return hp != maxHp; }

    }

    public int CurrentHealth { get { return hp; } }

    //center of mass
    public Vector3 COM 
    {
        get { return com.transform.position; }
    }

    #endregion

    //sets baseSpeed to match speed provided on start
    private void Start()
    {
        baseSpeed = speed;
        maxHp = hp;
        dashParticles.Stop();
        UpdatePlayerHp();
        cam.XSen = SaveManager.instance.gameData.xSen;
        cam.YSen = SaveManager.instance.gameData.ySen;
    }

    void Update()
    {
        //Compares velocity of player set between 0 and 1 to a float
        if (move.normalized.magnitude > 0.9f && !isPlayingSteps)
        {
            StartCoroutine(playSteps());
        }
        Movement();

        if (!isShooting && Input.GetButtonDown("Shoot"))
            StartCoroutine(Shoot());

        if (Time.time > speedTimer && speed > baseSpeed)
            ReduceSpeed();
    }

    #region MovementMethods
    void Movement()
    {
        ResetJump();

        move = (transform.right * Input.GetAxis("Horizontal")) +
               (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * speed * Time.deltaTime);

        Jump();

    }

    //used in movement for jump input
    void Jump()
    {

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            velocity.y = jumpHeight;
            jumpCount++;
            audioPlayer.PlayOneShot(audPlayerJump[Random.Range(0, audPlayerJump.Length)], audPlayerJumpVol);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //Resets Jump Count once Grounded
    void ResetJump()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpCount = 0;
        }

    }

    IEnumerator playSteps()
    {
        
        //is the player grounded?
        if (controller.isGrounded)
        {
            isPlayingSteps = true;
            audioPlayer.PlayOneShot(audPlayerSteps[Random.Range(0, audPlayerSteps.Length)], audPlayerStepsVol);

            if (speed < 10)
            {
                yield return new WaitForSeconds(0.5f);
            }
            else if (speed < 20)
            {
                yield return new WaitForSeconds(0.3f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
            isPlayingSteps = false;
        }
        
    }

    #endregion

    #region DamageMethods

    IEnumerator Shoot()
    {
        isShooting = true;
        RaycastHit hit;

        gunFlash.Play();

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {
            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().TakeDamage(shootDamage);
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }


    public void HealPlayer(int amount) 
    {
        if (hp + amount >= maxHp)
        {
            hp = maxHp;
        }
        else 
        {
            hp += amount;
        }

        UpdatePlayerHp();

    }

    public void TakeDamage(int amount, Vector3 pos)
    {
        //audioPlayer.PlayOneShot(audPlayerTakesDamage[Random.Range(0, audPlayerTakesDamage.Length)], audPlayerTakesDamageVol);
        //healthpack uses takedamage in negative amounts to heal
        if (hp - amount >= maxHp)
        {
            hp = maxHp;
        }
        else
        {
            hp -= amount;
        }

        //Angle of bullet
        float angleToPlayer = Vector3.SignedAngle(pos, transform.forward, Vector3.up);

        //TODO Add direction flash to screen.
        if (angleToPlayer >= -40 && angleToPlayer <= 40)
            StartCoroutine(GameManager.instance.TopFlash());
        else if (angleToPlayer <= -41 && angleToPlayer >= -130)
            StartCoroutine(GameManager.instance.RightFlash());
        else if (angleToPlayer >= 41 && angleToPlayer <= 130)
            StartCoroutine(GameManager.instance.LeftFlash());
        else
            StartCoroutine(GameManager.instance.BottomFlash());

        //Debug.Log(angleToPlayer);

        UpdatePlayerHp();

        if (hp <= 0)
            GameManager.instance.PlayerDeath();
    }
    #endregion

    public void UpdatePlayerHp() 
    {
        GameManager.instance.playerHpBar.fillAmount = (float)hp / (float)maxHp;        
    }

    #region SpeedMethods

    public void SpeedBoost(int boost, int offset)
    {
        speed += boost;
        speedTimer = Time.time + offset;
        dashParticles.Play();
    }

    void ReduceSpeed()
    {
        speedTimer = Time.time + 1f;

        if (speed - reduceRate <= baseSpeed)
        {
            speed = baseSpeed;
            dashParticles.Stop();
        }            
        else
            speed -= reduceRate;

    }

    #endregion

}
