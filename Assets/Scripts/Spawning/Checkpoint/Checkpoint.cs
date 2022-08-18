using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool cameraFollow;    // Determines whether the camera should continiously be following us or if it should be static
    
    public int level; // I forgot what this was for but its important
    public Vector2 cameraPosition; // The position out camera should be on (if the camera is static of course)
    public Vector2 playerLocation; // The position our player should respawn at.

    // Camera resizing
    public float cameraSize = 5.3f;

    public bool reversedGravity;

    private float cameraSizeSpeedPercentage = 0.009f; // x percent


    private bool alreadyRan = false; 
    private bool stopMovingTheFuckingCamera = true; // I'm a professional
    
    // Level snapping
    private float cameraSpeed = 6f; // Our hardcoded camera speed for snapping
    private float cameraSpeedIncrease = 1.025f;
    private float initialCameraSpeed;

    private GameObject player;


    // Camera follow
    private float cameraDistanceFromPlayer = 2f;

    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        initialCameraSpeed = cameraSpeed;
    }
        

    private void OnTriggerEnter2D(Collider2D other) 
    {

        // This may only run once

        // We dont want to set the same checkpoint again. Kinda pointless
        // I mean we could but like what's the point?


        if (other.name.Equals("Player") && !alreadyRan)
        {
            // We set a new checkpoint
            print("new checkpoint set");
            CheckpointData checkpointData = new CheckpointData();
            checkpointData.camerafollow=cameraFollow;
            checkpointData.cameraPosition=new Vector3(cameraPosition.x, cameraPosition.y, -10);
            checkpointData.level=level;
            checkpointData.location=playerLocation;
            checkpointData.reversedGravity=reversedGravity;
            CheckpointData.SetCheckpoint(checkpointData);
        
            // Now we gotta snap the camera to the new checkpoint
            alreadyRan=true;
            stopMovingTheFuckingCamera=false;
            
          
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            alreadyRan = false;
        }
    }



    private void Update() 
    {
        // We only want to run this if this really is the active checkpoint
        // We dont want multiple checkpoints fucking each other
        // This is a very shitty way of checking
        // We should be comparing objects instead.
        // But I guess this will suffice.
        if (CheckpointData.returnCheckpoint().location != playerLocation)
        {
            return;
        }


        // I'm not copying celeste :))

        // If camera follow is off (so a static camera)
        if (!CheckpointData.returnCheckpoint().camerafollow && !stopMovingTheFuckingCamera) 
        {
            Vector3 uwu = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(cameraPosition.x, cameraPosition.y, -10f), cameraSpeed * Time.deltaTime);
            Camera.main.transform.position = uwu;
            cameraSpeed *= cameraSpeedIncrease;
        
            if ( (Vector2) Camera.main.transform.position == cameraPosition)
            {
                stopMovingTheFuckingCamera=true;
                cameraSpeed = initialCameraSpeed;
            }
        
        }    

        // if our camera follow is on (so not static)
        if (CheckpointData.returnCheckpoint().camerafollow)
        {
            // This wont fit on one line you know
            
            Vector3 uwu = Vector3.MoveTowards(Camera.main.transform.position
            ,new Vector3(player.transform.localPosition.x + cameraDistanceFromPlayer, player.transform.localPosition.y+2.4f, -10f) // 2.4 another hardcoded value huh
            , cameraSpeed * Time.deltaTime);
            


            Camera.main.transform.position = uwu;
            cameraSpeed *= cameraSpeedIncrease;

        }

        // Makes our camera increase/decrease size
        if (Camera.main.orthographicSize != cameraSize) 
        {

            if (Camera.main.orthographicSize < cameraSize)
            {
                float size = Camera.main.orthographicSize * (1 + cameraSizeSpeedPercentage);
                Camera.main.orthographicSize = size;
                if (Camera.main.orthographicSize >= cameraSize)
                {
                    Camera.main.orthographicSize = cameraSize;
                }        
            }
            else 
            {
                float size = Camera.main.orthographicSize * (1 - cameraSizeSpeedPercentage);
                Camera.main.orthographicSize = size;
                if (Camera.main.orthographicSize <= cameraSize)
                {
                    Camera.main.orthographicSize = cameraSize;
                }
            }

        }


    
    }

    

}
