using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundcheckCollisions : MonoBehaviour
{
    [HideInInspector] public bool isTouching = false;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Ground")
        {
            setTouching(true);
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {

        if (other.tag == "Ground")
        {
            setTouching(false);
        }    
    }

    private void setTouching(Boolean value) {
        isTouching = value;
    }


}
