using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumping : MonoBehaviour
{
    public Transform leftSide;
    public Transform rightSide;
    public Vector2 wallJumpforce;
    public LayerMask whatIsWall;
    public float wallCheckRadius = 0.1f;
    public float blockMovementTime = 0.2f;
    
    public GameObject groundCheck;
    public LayerMask WhatIsGround;
    public float checkRadius;
    

    private bool wallAtLeftSide;
    private bool wallAtRightSide;
    private bool jumpToLeft;
    private bool jumpToRight;
    private bool isWallJumping = false;
    private bool blockMovementToLeft = false;
    private bool blockMovementToRight = false;
    private Rigidbody2D rb;
    private PlayerStates playerStates;
    

    private void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
    }

    private void FixedUpdate()
    {
        // This is for the walljumping
        // It blocks movement to the side you're trying to move towards when walljumping for x seconds
        // I was thinking that maybe instead of blocking movement we should add an extra force to compensate for the counterforce but I guess this works just fine
        float moveInput = Input.GetAxis("Horizontal");
        if (blockMovementToLeft && moveInput < 0)
        {
            moveInput = 0;

        }
        if (blockMovementToRight && moveInput > 0)
        {
            moveInput = 0;
        }

        // ----- Walljumping
        if (jumpToLeft)
        {
            rb.velocity = new Vector2(0, 0);
            jumpToLeft = false;
            rb.velocity += (new Vector2(-1 * wallJumpforce.x, wallJumpforce.y * playerStates.jumpDirection.y));

        }
        if (jumpToRight)
        {
            rb.velocity = new Vector2(0, 0);
            jumpToRight = false;
            rb.velocity += (new Vector2(1 * wallJumpforce.x, wallJumpforce.y * playerStates.jumpDirection.y));
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

        // Sets walljumping to false (WOW)
        isWallJumping = false;
        playerStates.isWalljumping = false;

        playerStates.blockMovementToLeft = false;
        playerStates.blockMovementToRight = false;

    }
}
