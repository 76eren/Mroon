using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumping : MonoBehaviour
{
    [SerializeField] private Transform leftSide;
    [SerializeField] private Transform rightSide;
    [SerializeField] private Vector2 wallJumpforce;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private float wallCheckRadius = 0.1f;
    [SerializeField] private float blockMovementTime = 0.2f;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private float checkRadius;

    private bool wallAtLeftSide;
    private bool wallAtRightSide;
    private bool jumpToLeft;
    private bool jumpToRight;
    private bool blockMovementToLeft = false;
    private bool blockMovementToRight = false;
    private Rigidbody2D rb;
    private PlayerStates playerStates;
    private Boolean isWallJumping;
    

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
    }

    private void FixedUpdate()
    {
        // This is for the walljumping
        // It blocks movement to the side you're trying to move towards when walljumping for x seconds
        // Alternatively we could add counter force to said direction for x seconds instead of blocking movement for that duaration, but this is fine for now
        float moveInput = Input.GetAxis("Horizontal");
        if (blockMovementToLeft && moveInput < 0 || blockMovementToRight && moveInput > 0)
        {
            moveInput = 0;
        }

        // ----- Walljumping
        if (jumpToLeft || jumpToRight)
        {
            rb.velocity = Vector2.zero;
            int directionMultiplier = jumpToLeft ? -1 : 1;
            if (jumpToLeft) jumpToLeft = false;
            if (jumpToRight) jumpToRight = false;
            rb.velocity += new Vector2(directionMultiplier * wallJumpforce.x, wallJumpforce.y * playerStates.jumpDirection.y);
        }
    }


    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, WhatIsGround);
        if (isGrounded)
        {
            isWallJumping = false;
            playerStates.isWalljumping = false;
        }

        // Walljumping
        wallAtRightSide = Physics2D.OverlapCircle(rightSide.position, wallCheckRadius, whatIsWall);
        wallAtLeftSide = Physics2D.OverlapCircle(leftSide.position, wallCheckRadius, whatIsWall);


        // This makes sure we don't walljump when we're standing ON the wall.
        // I don't know why this is working but it is and that really says something....
        if (groundCheck.GetComponent<GroundcheckCollisions>().isTouching)
        {
            wallAtLeftSide = false;
            wallAtRightSide = false;

            playerStates.isWalljumping = false;
            playerStates.dashes = playerStates.originalDash;
        }

        if (wallAtLeftSide && Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            playerStates.isWalljumping = true;
            isWallJumping = true;
            playerStates.isWalljumping = true;

            jumpToRight = true;
            blockMovementToLeft = true;
            playerStates.blockMovementToLeft = true;
            StartCoroutine(stopBlockInput());
        }

        if (wallAtRightSide && Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            playerStates.isWalljumping = true;
            isWallJumping = true;
            playerStates.isWalljumping = true;
            playerStates.blockMovementToRight = true;

            jumpToLeft = true;
            blockMovementToRight = true;
            StartCoroutine(stopBlockInput());
        }


    }

    IEnumerator stopBlockInput()
    {
        yield return new WaitForSeconds(blockMovementTime);
        blockMovementToLeft = false;
        blockMovementToRight = false;

        isWallJumping = false;
        playerStates.isWalljumping = false;

        playerStates.blockMovementToLeft = false;
        playerStates.blockMovementToRight = false;

    }
}
