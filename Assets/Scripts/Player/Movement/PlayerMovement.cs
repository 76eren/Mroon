using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject groundCheck;
    public LayerMask WhatIsGround;
    public float speed;
    public float checkRadius;
    public float jumpForce;
    public float counterForce;

    public Transform leftSide;
    public Transform rightSide;

    private float previousInput = 0f;
    private float currentLookSide;
    private bool jump = false;

    private PlayerStates playerStates;
    


    private void Start() {
        playerStates = GetComponent<PlayerStates>();
        rb = GetComponent<Rigidbody2D>();
        currentLookSide = transform.localScale.x;
        
    }

    void FixedUpdate()
    {
        // Jumping
        if (jump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            rb.velocity +=  GetComponent<PlayerStates>().jumpDirection * jumpForce;            
            jump = false;
        }


        if (!Input.GetButton("Horizontal") && !playerStates.isDashing && !playerStates.isWalljumping)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        
        float moveInput = Input.GetAxis("Horizontal");

        if (moveInput < 0 && playerStates.blockMovementToLeft)
        {
            moveInput = 0;
        }        
        if (moveInput > 0 && playerStates.blockMovementToRight)
        {
            moveInput = 0;
        }

        // This bit handles the movement
        previousInput = moveInput;
        rb.velocity += new Vector2(moveInput * speed, 0); // Moves our player

        // flips our player
        if (moveInput < 0 && currentLookSide > 0)
        {
            Vector2 lookDirection = transform.localScale;
            lookDirection.x *= -1;
            transform.localScale = lookDirection;


            // We also need to change the positions of the leftside and rightside because they are 
            // children of the character
            // So they also will get flipped in the process
            Vector2 left_side = leftSide.transform.position;
            Vector2 right_side = rightSide.transform.position;

            leftSide.transform.position = right_side;
            rightSide.transform.position = left_side;


            currentLookSide *= -1;

        }
        else if (moveInput > 0 && currentLookSide < 0)
        {
            Vector2 lookDirection = transform.localScale;
            lookDirection.x = 0.9f;
            transform.localScale = lookDirection;


            // We also need to change the positions of the leftside and rightside because they are 
            // children of the character
            // So they also will get flipped in the process
            Vector2 left_side = leftSide.transform.position;
            Vector2 right_side = rightSide.transform.position;

            leftSide.transform.position = right_side;
            rightSide.transform.position = left_side;
            currentLookSide *= -1;

        }     

        // This bit sets a speed cap
        // We do this by adding a counter force in the opposite direction we're moving in
        // NOTE: This only makes sure our player doesn't go too fast on the x-axis.
        // You could do the same on the y-axis but I don't think that is necessary
        
        float velocityx = rb.velocity.x;
        float forceBack = 0f;
        if (velocityx > 0)
        {
            forceBack = -(velocityx - playerStates.speedCap);
        }
        else if (velocityx < 0)
        {
            forceBack = -(velocityx+ playerStates.speedCap);
        }
        if (velocityx > playerStates.speedCap || velocityx < -playerStates.speedCap)
        {
            rb.velocity += new Vector2(forceBack, 0f);
        }

    }

    private void Update() 
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, WhatIsGround);

        
        // When we jump there is this small window where we are actually still "touching" the ground
        // This is caused because we are still in the radius of the sphere that is being created at the player's feet
        // We can fix this by just doing all of these if statements which I am too lazy to explain,
        if (isGrounded && playerStates.jumps != playerStates.originalJumps) 
        {
            if (!Input.GetKey(KeyCode.Space)) 
            {
                playerStates.jumps = playerStates.originalJumps;

            }
                    
        }

        if (Input.GetKeyDown(KeyCode.Space) && playerStates.jumps > 0 && !playerStates.parryJump) {
            jump=true;
            playerStates.jumps--;
               
        }

    }

}
