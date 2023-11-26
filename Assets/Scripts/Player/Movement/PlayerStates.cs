// A universal script on the players
// Containing all of the player's states and shit
// I made this script so all movement related script don't need to constantly reference each other

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour
{
    // Movement
    public float speedCap = 8f;
    [HideInInspector] public float originalCap;

    // Dashing
    public int dashes = 1;
    [HideInInspector] public int originalDash;

    // Parrying
    [HideInInspector] public bool parryJump = false; // Determines whether we're able to jump or not; used for parrying.

    // For checking I guess
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isWalljumping = false;


    // Movement
    [HideInInspector] public bool blockMovementToLeft = false;
    [HideInInspector] public bool blockMovementToRight = false;

    // Juping
    public int jumps;
    
    [HideInInspector] public int originalJumps;

    [HideInInspector] public Vector2 jumpDirection = Vector2.up; // vecyor2.up or vector2.down. 


    // Gravity scale
    // I really think this gravityScale can just be removed 
    // It is already stored in the rb.gravityscale so there would be no reason to store it a second time
    // A lot of things depend on this now so I'm not going to change it now
    // TODO: there is no need to keep track of this here

    // Note:
        // - The dashing.cs script does NOT edit the value of the gravityscale, which should have been the proper behavior since the beginning
                // this may lead to issues at some point but right now it's fine.
    [HideInInspector] public float gravityScale;



    private void Awake()
    {
        originalCap = speedCap;
        originalDash = dashes;
        originalJumps = jumps;
        gravityScale = GetComponent<Rigidbody2D>().gravityScale;
    }
}
