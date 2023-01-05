using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] int speed;

    Vector3 move;
    Vector3 velocity;

    [SerializeField] float gravity = 9.8f;
    [SerializeField] int jumpHeight;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpCount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement() 
    {

        move = (transform.right * Input.GetAxis("Horizontal")) + 
               (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * speed * Time.deltaTime);

        Jump();

    }


    //used in movement for jump input
    void Jump() 
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            velocity.y = jumpHeight;
            jumpCount++;
        }

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}
