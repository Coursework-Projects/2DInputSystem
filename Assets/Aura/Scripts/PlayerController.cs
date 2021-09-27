using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlatformActions pActions;

    [Header("Movement Variables")]
    [SerializeField] float m_Speed, m_JumpSpeed;

    [SerializeField]LayerMask groundMask;
    Collider2D m_Collider;
    Rigidbody2D m_RigidBody;

    private void Awake()
    {
        pActions = new PlatformActions();
        pActions.Land.Jump.performed += _=> Jump();

        m_Collider = GetComponent<Collider2D>();
        m_RigidBody = GetComponent<Rigidbody2D>();
    }

    private void Jump()
    {
        //add a force to players rigidbody if player is grounded
        if(IsGrounded())
        {
            m_RigidBody.AddForce(Vector2.up * m_JumpSpeed, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        //Use collider to get player bounds
        //check if the bounds overlap the ground
        //if they do then IsGrounded is true
        Vector2 topLeftCorner = transform.position;
        Vector2 bottomRightCorner = transform.position;

        topLeftCorner.x  -= m_Collider.bounds.size.x;
        topLeftCorner.y -= m_Collider.bounds.size.y;

        bottomRightCorner.x += m_Collider.bounds.size.x;
        bottomRightCorner.y += m_Collider.bounds.size.y;

        return Physics2D.OverlapArea(topLeftCorner, bottomRightCorner, groundMask);
       
    }

    private void OnEnable()
    {
        pActions.Enable();
    }

    private void OnDisable()
    {
        pActions.Disable();
    }

    private void Update()
    {
        //Read the movement value
        float movementInput = pActions.Land.Move.ReadValue<float>();

        //Move the player
        Vector3 currentPosition = transform.position;
        currentPosition.x += movementInput * m_Speed * Time.deltaTime;
        transform.position = currentPosition;
    }
}
