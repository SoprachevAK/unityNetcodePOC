using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{

    public float moveSpeed = 10;
    public float ratationSpeed = 10;

    Vector2 inputMove;
    bool inputJump;
    bool inputSprint;

    bool grounded = false;
    float verticalVelocity = 0;


    CharacterController characterController;
    Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    override public void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(GetComponent<PlayerInput>());
        }
    }



    void Update()
    {
        CheckGrounded();
        if (IsOwner)
        {
            Move();
        }
    }

    void CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position + Vector3.up * 0.05f, Vector3.down, 0.1f);
        Debug.DrawRay(transform.position, Vector3.down * 0.1f, grounded ? Color.green : Color.red);
    }

    void Move()
    {
        Vector3 move = new Vector3(0, 0, inputMove.y * (inputSprint ? 2 : 1));
        move = transform.TransformDirection(move);
        animator.SetBool("Grounded", grounded);

        if (grounded)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("FreeFall", false);

            if (inputJump)
            {
                verticalVelocity = -Mathf.Sqrt(1.5f * 2f * 15f);
                animator.SetBool("Jump", true);
            }
        }
        else
        {
            inputJump = false;
            verticalVelocity += 15f * Time.deltaTime;
        }


        transform.Rotate(0, inputMove.x * ratationSpeed * Time.deltaTime, 0);
        characterController.Move(move * moveSpeed * Time.deltaTime - new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(inputMove.y * moveSpeed * (inputSprint ? 2 : 1)));
        animator.SetFloat("MotionSpeed", Mathf.Sign(inputMove.y) * characterController.velocity.magnitude / (inputSprint ? 4 : 2));
    }

    void OnFootstep(AnimationEvent animationEvent) { }
    void OnLand(AnimationEvent animationEvent) { }

    #region INPUT
    public void InputMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>();
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputJump = true;
        }
    }

    public void InputSprint(InputAction.CallbackContext context)
    {
        inputSprint = context.ReadValue<float>() > 0;
    }
    #endregion
}
