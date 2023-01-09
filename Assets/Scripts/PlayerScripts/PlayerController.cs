using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("----- Cpomonents -----")]
    [SerializeField] CharacterController controller;

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
    bool isShooting = false;
    [Range(5, 50)] [SerializeField] int shootDamage = 10;
    [Range(15, 200)] [SerializeField] int shootDist;
    [Range(0.1f, 2)] [SerializeField] float shootRate;

    Vector3 move;
    Vector3 velocity;

    #region Properties

    public bool CanHeal
    {
        get { return hp != maxHp; }

    }

    #endregion

    //sets baseSpeed to match speed provided on start
    private void Start()
    {
        baseSpeed = speed;
    }

    void Update()
    {
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


    #endregion

    #region DamageMethods

    IEnumerator Shoot()
    {
        isShooting = true;
        RaycastHit hit;

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

    public void TakeDamage(int amount)
    {
        //healthpack uses takedamage in negative amounts to heal
        if (hp - amount >= maxHp)
            hp = maxHp;
        else
            hp -= amount;

        Debug.Log(hp);
    }
    #endregion

    #region SpeedMethods

    public void SpeedBoost(int boost, int offset)
    {
        speed += boost;
        speedTimer = Time.time + offset;
    }

    void ReduceSpeed()
    {
        speedTimer = Time.time + 1f;

        if (speed - reduceRate <= baseSpeed)
            speed = baseSpeed;
        else
            speed -= reduceRate;

    }

    #endregion

}
