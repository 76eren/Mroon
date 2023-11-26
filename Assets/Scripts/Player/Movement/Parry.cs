using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 ParryForce;
    [SerializeField] private float SpeedCapIncrease = 7f;
    [SerializeField] private float speedCapDecrease = 0.5f;
    [SerializeField] private float delay = 0.2f;

    private bool checkForJump = false;
    private float oldCap;
    private PlayerStates playerStates;
    private bool checkWhenToEnableJumpingAgain = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStates = GetComponent<PlayerStates>();
        oldCap = playerStates.speedCap;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Parry"))
        {
            checkForJump = true;
            playerStates.parryJump = true; // Makes us unable to perform a regular jump and instead allow us to perform a custom (parry) jump
            checkWhenToEnableJumpingAgain=false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Parry"))
        {
            checkForJump = false;
            checkWhenToEnableJumpingAgain=true;
        }
    }


    private void Update()
    {
        
        // After doing a parryjump if the user keeps holding down the jump button the player will perform a regular jump right after
        // A parry jump (because they left the collider). This causes the player to lose their regular jumps when parrying.
        // This here will make sure that doesn't hapepn.
        if (checkWhenToEnableJumpingAgain && !Input.GetKeyDown(KeyCode.Space)) 
        {
            checkWhenToEnableJumpingAgain=false;
            playerStates.parryJump=false;
        }


        if (checkForJump && Input.GetKeyDown(KeyCode.Space))
        {
            // we reset the velocity so the current forces don't add up
            rb.velocity = new Vector2(rb.velocity.x, 0);

            StartCoroutine(EditSpeedCap());
            if (transform.localScale.x > 0)
            {
                // We parry up to the right
                rb.velocity += (ParryForce * playerStates.jumpDirection.y);

                // We play animation
                GetComponent<Animator>().Play("ParryRight");

            }
            else if (transform.localScale.x < 0)
            {
                // we parry up to the left
                rb.velocity += (new Vector2(ParryForce.x * -1, ParryForce.y * playerStates.jumpDirection.y));

                // We play animation
                GetComponent<Animator>().Play("ParryLeft");
            }

            // Parrying also resets our dashes
            playerStates.dashes = playerStates.originalDash;

        }

    }

    IEnumerator EditSpeedCap()
    {
        playerStates.speedCap += SpeedCapIncrease;
        while (true)
        {
            yield return new WaitForSeconds(delay);
            playerStates.speedCap -= speedCapDecrease;

            if (playerStates.speedCap <= oldCap)
            {
                playerStates.speedCap = oldCap;
                                
                break;
            }


        }

    }







}


