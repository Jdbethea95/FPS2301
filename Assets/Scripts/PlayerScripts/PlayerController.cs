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
    float speedTimer;
    int baseSpeed;

    [SerializeField] int pushBackTime;
    [SerializeField] int dashDistance;
    [SerializeField] int dashCount;


    [Header("----- Jump Stats -----")]
    [SerializeField] float gravity = 9.8f;
    [Range(5, 10)] [SerializeField] int jumpHeight;
    [Range(1, 3)] [SerializeField] int jumpMax;
    [SerializeField] int jumpCount;
    [SerializeField] float coyoteOffset;
    [SerializeField] float coyoteSphereHeight;

    [Header("----- Gun Stats -----")]
    [Range(1, 50)] [SerializeField] int shootDamage = 10;
    [Range(15, 200)] [SerializeField] int shootDist;
    [Range(0.1f, 2)] [SerializeField] float shootRate;
    [SerializeField] float overHeat;
    [SerializeField] float overHeatMax;
    [SerializeField] int overHeatReduction;
    [SerializeField] int overHeatCooldown;
    [SerializeField] float heatAmount = 1.0f;
    float overHeatTimer;
    float overheatHolder;
    int damage;

    [Header("----- Gun Components -----")]
    [SerializeField] ParticleSystem gunFlash;
    [SerializeField] ParticleSystem gunSpark;
    [SerializeField] GameObject gunModel;
    [SerializeField] ParticleSystem.MainModule gunFlashColor;
    [SerializeField] Animator animGun;

    [Header("----- Audio -----")]
    [SerializeField] AudioClip[] audPlayerTakesDamage;
    [Range(0, 1)] [SerializeField] float audPlayerTakesDamageVol;
    [SerializeField] AudioClip[] audPlayerJump;
    [Range(0, 1)] [SerializeField] float audPlayerJumpVol;
    [SerializeField] AudioClip[] audPlayerSteps;
    [Range(0, 1)] [SerializeField] float audPlayerStepsVol;
    [SerializeField] AudioClip[] audPlayerShoot;
    [SerializeField] AudioClip[] ogPlayerShootAud;
    [Range(0, 1)] [SerializeField] float audPlayerShootVol;
    [SerializeField] AudioClip overheatAud;



    #region Co-Bools
    bool isShooting = false;
    bool isPlayingSteps;
    bool isPlayingShootAudio;
    bool isSparking;
    bool isOverheated;
    bool zeroedHeat;
    #endregion

    #region OgStats
    int ogShootDist;
    float ogShootRate;
    int ogShootDamage;
    int ogHp;
    int ogSpeed;
    float ogOverHeatMax; //DevTool
    ParticleSystem.MinMaxGradient ogColor;
    [SerializeField] Mesh ogGunModel;
    [SerializeField] Material ogMaterial;
    #endregion

    #region PerkVars
    string perkId;
    string perkName;
    bool foundPerk;
    #endregion

    #region MoveVectors
    public Vector3 pushBack;
    Vector3 move;
    Vector3 velocity;
    public Vector3 hitDirection = Vector3.zero;
    #endregion

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

    public bool PerkFound { get { return foundPerk; } }
    public string PerkID { get { return perkId; } }

    public string PerkName { get { return perkName; } }

    public int BaseSpeed { get { return baseSpeed; } }

    #endregion


    //sets baseSpeed to match speed provided on start
    private void Start()
    {
        #region OgStatSave
        //sets base stats
        baseSpeed = speed;
        maxHp = hp;
        ogShootDist = shootDist;
        ogShootRate = shootRate;
        ogShootDamage = shootDamage;
        ogHp = maxHp;
        ogSpeed = baseSpeed;
        gunFlashColor = gunFlash.main;
        ogColor = gunFlash.main.startColor;
        ogOverHeatMax = overHeatMax; //devTool 
        #endregion


        foundPerk = false;

        dashParticles.Stop();
        UpdatePlayerHp();
        UpdateHeatBar();

        //checks for lobby level then activates any perks attached, if any.
        if (PerkManager.instance.activePerks[0] != null || PerkManager.instance.activePerks[1] != null ||
            PerkManager.instance.activePerks[2] != null)
        {

            DeActivatePerks();
            ActivatePerks();
        }

        #region SettingsUpdate
        cam.XSen = SaveManager.instance.gameData.xSen;
        cam.YSen = SaveManager.instance.gameData.ySen;
        audPlayerJumpVol = SaveManager.instance.gameData.sfxVol;
        audPlayerShootVol = SaveManager.instance.gameData.sfxVol;
        audPlayerStepsVol = SaveManager.instance.gameData.sfxVol;
        audPlayerTakesDamageVol = SaveManager.instance.gameData.sfxVol;
        #endregion

    }

    void Update()
    {

        if (!GameManager.instance.isPaused)
        {
            //resolved the pushback.
            PushBackReduction();

            //Compares velocity of player set between 0 and 1 to a float
            if (move.normalized.magnitude > 0.9f && !isPlayingSteps)
                StartCoroutine(playSteps());

            Movement();

            if (Input.GetButtonDown("Sprint") && dashCount > 0)
                Dash();


            if (!isShooting && Input.GetButtonDown("Shoot") && !GameManager.instance.isPaused && !isOverheated)
                StartCoroutine(Shoot());

            //checks the timer for the overheat plus the cooldown time to see if it can reduce bar.
            if (Time.time > overHeatTimer)
                ReduceHeat();

            if (Time.time > speedTimer && speed > baseSpeed)
                ReduceSpeed();
        }
    }


    #region MovementMethods
    void Movement()
    {
        ResetJump();

        move = (transform.right * Input.GetAxis("Horizontal")) +
               (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * speed * Time.deltaTime);

        //Gun Animation
        float animSpeed = (Input.GetAxis("Vertical") * speed) * 0.1f;
        if (animSpeed < 0)
            animSpeed *= -1;

        animGun.SetFloat("GunBob", animSpeed);



        damage = Mathf.FloorToInt(controller.velocity.magnitude);
        //Debug.Log(Mathf.FloorToInt(controller.velocity.magnitude));

        //pushback implementation inside Jump

        Jump();

    }

    #region JumpMethods
    //used in movement for jump input
    void Jump()
    {

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax && IsGrounded())
        {
            velocity.y = jumpHeight;
            jumpCount++;
            //audioPlayer.PlayOneShot(audPlayerJump[Random.Range(0, audPlayerJump.Length)], audPlayerJumpVol);
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move((velocity + pushBack) * Time.deltaTime);


    }


    bool IsGrounded()
    {

        Vector3 pos = new Vector3(transform.position.x, transform.position.y - coyoteSphereHeight,
                                   transform.position.z);

        if (Physics.CheckSphere(pos, coyoteOffset, 3))
        {
            return true;
        }

        return false;
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
    #endregion

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

    void PushBackReduction()
    {
        pushBack.x = Mathf.Lerp(pushBack.x, 0, Time.deltaTime * pushBackTime);
        pushBack.y = Mathf.Lerp(pushBack.y, 0, Time.deltaTime * pushBackTime * 3);
        pushBack.z = Mathf.Lerp(pushBack.z, 0, Time.deltaTime * pushBackTime);
    }

    void Dash()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            pushBack = (transform.forward) * (Input.GetAxis("Vertical") * dashDistance);
            SpeedBoost(3, 1);
            GameManager.instance.ReduceBoost();
            dashCount--;
        }
        else if (Input.GetAxis("Horizontal") != 0)
        {
            pushBack = (transform.right) * (Input.GetAxis("Horizontal") * dashDistance);
            SpeedBoost(3, 1);
            GameManager.instance.ReduceBoost();
            dashCount--;
        }



    }

    public void DashPointGain()
    {
        if (dashCount < 3)
        {
            dashCount++;
            GameManager.instance.GainBoost();
        }

    }

    #endregion


    #region DamageMethods

    IEnumerator Shoot()
    {
        isShooting = true;
        RaycastHit hit;

        //adds 1 to over heat if not maxed. overheats when max is reached.
        if (overHeat < overHeatMax)
        {
            overHeat += heatAmount;
            UpdateHeatBar();
        }
        else if (overHeat >= overHeatMax)
        {
            isOverheated = true;
            animGun.SetTrigger("OverHeat");
            audioPlayer.PlayOneShot(overheatAud, audPlayerShootVol);
            //affects heat amount if not zeroed when fired by the player.
            zeroedHeat = false;
            overHeat = overHeatMax;
        }


        //adds time to the timer before the bar will reduce.
        overHeatTimer = Time.time + overHeatCooldown;

        //plays the particle effect infront of gun model.
        gunFlash.Play();
        animGun.SetTrigger("Fire");

        if (!isPlayingShootAudio)
            StartCoroutine(PlayShootAudio());


        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
        {

            if (!hit.collider.CompareTag("Bullet"))
            {
                gunSpark.transform.position = hit.point;
                gunSpark.Play();

                if (!isSparking)
                    StartCoroutine(ResetSpark());
            }

            if (hit.collider.GetComponent<IDamage>() != null)
            {
                hit.collider.GetComponent<IDamage>().TakeDamage(shootDamage + (damage / 5));
                //Debug.Log(shootDamage + (damage / 5));
            }
        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator PlayShootAudio()
    {

        isPlayingShootAudio = true;
        audioPlayer.PlayOneShot(audPlayerShoot[Random.Range(0, audPlayerShoot.Length)], audPlayerShootVol);
        yield return new WaitForSeconds(.15f);
        isPlayingShootAudio = false;

    }

    IEnumerator ResetSpark()
    {
        isSparking = true;
        yield return new WaitForSeconds(gunSpark.main.duration);
        isSparking = false;
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
        audioPlayer.PlayOneShot(audPlayerTakesDamage[Random.Range(0, audPlayerTakesDamage.Length)], audPlayerTakesDamageVol);
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
        {
            cam.isShaking = false;
            GameManager.instance.PlayerDeath();

        }

    }

    void ReduceHeat()
    {
        //holder is used to store the old heat value to see if its time to add time to timer
        if (overheatHolder == 0)
            overheatHolder = overHeat - overHeatReduction;


        //prevents the bar from emptying instantly.
        if (overHeat > 0)
            overHeat = Mathf.Lerp(overHeat, (overHeat - overHeatReduction), 10 * Time.deltaTime);
        else
            overHeat = 0;

        UpdateHeatBar();

        if (overheatHolder == overHeat)
        {
            overHeatTimer = Time.time + 0.05f;
            overheatHolder = 0;
        }


        if (overHeat <= 0)
        {
            isOverheated = false;
            heatAmount = 1f;
            zeroedHeat = true;
        }
        else if (overHeat <= (overHeatMax * 0.5f) && !zeroedHeat)
        {
            isOverheated = false;
            heatAmount = 3f;
        }


    }

    public void UpdateHeatBar()
    {
        GameManager.instance.overHeatBar.fillAmount = overHeat / overHeatMax;
    }

    public void UpdatePlayerHp()
    {
        GameManager.instance.playerHpBar.fillAmount = (float)hp / (float)maxHp;
    }


    #endregion


    #region SpeedMethods

    public void SpeedBoost(int boost, int offset)
    {
        speed += boost;
        speedTimer = Time.time + offset;
        dashParticles.Play();
    }

    public void SetSpeed(int amount)
    {
        speed = amount;
    }

    public void StopDashPart()
    {
        dashParticles.Stop();
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

    #region PerkMethods

    public void PerkPickup(SO_Perk pickup)
    {
        perkId = pickup.ID;
        perkName = pickup.perkName;
        foundPerk = true;
    }


    public void AddPerk(SO_Perk perk)
    {
        for (int i = 0; i < 3; i++)
        {
            if (PerkManager.instance.activePerks[i] != null)
            {
                PerkManager.instance.activePerks[i] = perk;
                break;
            }
        }
    }

    public void ClearPerks()
    {
        for (int i = 0; i < 3; i++)
        {
            PerkManager.instance.activePerks[i] = null;
        }
    }

    public void ActivatePerks()
    {

        for (int i = 0; i < 3; i++)
        {
            if (PerkManager.instance.activePerks[i] != null)
            {

                maxHp += PerkManager.instance.activePerks[i].hpModifier;
                hp += PerkManager.instance.activePerks[i].hpModifier;

                baseSpeed += PerkManager.instance.activePerks[i].SpeedModifier;
                speed += PerkManager.instance.activePerks[i].SpeedModifier;

                shootDamage += PerkManager.instance.activePerks[i].ShootDamage;
                shootDist += PerkManager.instance.activePerks[i].ShootDistance;
                shootRate += PerkManager.instance.activePerks[i].ShootRate;

                overHeatMax += PerkManager.instance.activePerks[i].overHeatMax;

                if (PerkManager.instance.activePerks[i].Model != null)
                {
                    gunModel.GetComponent<MeshFilter>().sharedMesh = PerkManager.instance.activePerks[i].Model;
                    continue;
                }



                if (PerkManager.instance.activePerks[i].material != null)
                {
                    Debug.Log("ThisHappened");
                    gunModel.GetComponent<MeshRenderer>().sharedMaterial = PerkManager.instance.activePerks[i].material;
                }

                if (PerkManager.instance.activePerks[i].isColored)
                {
                    gunFlashColor.startColor = PerkManager.instance.activePerks[i].color;
                }

            }
        }
    }
    public void DeActivatePerks()
    {
        maxHp = ogHp;
        hp = ogHp;

        baseSpeed = ogSpeed;
        speed = ogSpeed;

        shootDamage = ogShootDamage;
        shootDist = ogShootDist;
        shootRate = ogShootRate;

        overHeatMax = ogOverHeatMax;

        if (PerkManager.instance.activePerks[0] != null)
            gunModel.GetComponent<MeshFilter>().sharedMesh = ogGunModel;

        if (PerkManager.instance.activePerks[1] != null)
            gunModel.GetComponent<MeshRenderer>().sharedMaterial = ogMaterial;

        if (PerkManager.instance.activePerks[2] != null)
            gunFlashColor.startColor = ogColor;
    }


    #endregion

    public void UpdatePlayerSFX()
    {
        audPlayerJumpVol = SaveManager.instance.gameData.sfxVol;
        audPlayerShootVol = SaveManager.instance.gameData.sfxVol;
        audPlayerStepsVol = SaveManager.instance.gameData.sfxVol;
        audPlayerTakesDamageVol = SaveManager.instance.gameData.sfxVol;
        cam.XSen = SaveManager.instance.gameData.xSen;
        cam.YSen = SaveManager.instance.gameData.ySen;
    }

    #region DevTools

    public void MaxHealth()
    {
        hp = int.MaxValue;
        maxHp = int.MaxValue;
    }
    public void RevertHealth()
    {
        hp = ogHp;
        maxHp = ogHp;
    }

    public void MaxTheHeat()
    {
        overHeatMax = int.MaxValue;
    }

    public void BringTheHeat()
    {
        overHeat = 0;
        overHeatMax = ogOverHeatMax;
    }

    public void MaxDash()
    {
        dashCount = int.MaxValue;
    }
    public void RevertDash()
    {
        dashCount = 3;
    }

    #endregion
}
