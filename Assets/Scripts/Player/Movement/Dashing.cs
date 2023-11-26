using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private float speedCapIncrease = 8;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private float checkRadius;



    private bool isDashing = false;
    private bool canDash = false;
    private float oldCap;
    private float oldGravityScale;
    public float dashForce;
    private Rigidbody2D rb;
    private PlayerStates playerStates;
    private float gravityScale;

    public Transform leftSide;
    public Transform rightSide;        


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
        gravityScale = rb.gravityScale;

        oldCap = playerStates.speedCap;        
    }

    private void FixedUpdate()
    {
        if (canDash)
        {
            // Before we can dash we need to update the speedcap
            playerStates.speedCap += speedCapIncrease;
            gravityScale = rb.gravityScale;
            rb.gravityScale = 0;
            isDashing = true;
            rb.velocity = new Vector2(rb.velocity.x, 0f);


            if (transform.localScale.x > 0)
            {
                rb.AddForce(Vector2.right * dashForce);
            }
            else if (transform.localScale.x < 0)
            {
                rb.AddForce(Vector2.left * dashForce);
            }

            playerStates.dashes--;
            canDash = false;
            StartCoroutine(stopDashing(dashLength));
        }
    }

    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, checkRadius, WhatIsGround);
        if (isGrounded)
        {
            playerStates.dashes = playerStates.originalDash;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && playerStates.dashes > 0 && Input.GetButton("Horizontal"))
        {
            canDash = true;
            isDashing = true;
            playerStates.isDashing = true;
        }
    }

    IEnumerator stopDashing(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait); 
        playerStates.speedCap = oldCap;

        if (playerStates.gravityScale != 0) 
        {
            rb.gravityScale = playerStates.gravityScale;
        }

        else 
        {
            // Alright so let me explain
            // If we dash into a reverse block things mess up
            // The dashing sets the gravity scale to 0 and the Rotation script sets the gravityscale to the reverse of 0
            // This bit is supposed to make sure that doesn't happen.
            playerStates.gravityScale = gravityScale; // Sets the gravity scale to before we rotated
            
            // Rotates; we know we want to rotate because otherwise the gravityScale of the playerstates would not be 0
            // This script DOES NOT touch the Playerstates.gravityscale so if the playerstates.gravityscale is 0 we know we dashed into a reverse block
            Rotater.ReverseGravity(this.gameObject); 
        }
    

        isDashing = false;
        playerStates.isDashing = false;
    }


}
