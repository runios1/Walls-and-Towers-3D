using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public CharacterController controller;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    [Header("Animations")]
    public Animator animator;

    private bool jumpedOnce = false;
    // Update is called once per frame
    void Update()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        animator.SetBool("Ground",isGrounded);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            jumpedOnce=false;
            animator.SetBool("Jump", false);
        }

        if (Input.GetButtonDown("Jump") && isGrounded && !jumpedOnce)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            jumpedOnce=true;
            animator.SetBool("Jump", true);
        }
        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);


            // Update animator parameters
            animator.SetFloat("Horizontal", horizontal);
            animator.SetFloat("Vertical", vertical);
        }
        else
        {
            // If not moving, set parameters to 0
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
        }
    }
}
