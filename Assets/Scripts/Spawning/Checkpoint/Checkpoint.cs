using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool cameraFollow; // Determines whether the camera should continiously be following us or if it should be static
    public int level; 
    public Vector2 cameraPosition; // The position out camera should be on (if the camera is static)
    public Vector2 playerLocation; // The position our player should respawn at.


    // Camera resizing
    public float cameraSize = 5.3f;
    public bool reversedGravity;
    private float cameraSizeSpeedPercentage = 0.009f; // x percent
    private bool alreadyRan = false; 
    private bool stopMovingTheCamera = true; 
    

    // Level snapping
    // Thes cannot be public because then we'd need to change this for each gameobject
    private float cameraSpeed = 80f; 
    // private float cameraSpeed = 6f; 

    private float cameraSpeedIncrease = 70f;
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
        // We dont want to set the same checkpoint again.
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
            stopMovingTheCamera=false;
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
        // Sets fps to 60
        // Application.targetFrameRate = 60;

        // We only want to run this if this really is the active checkpoint
        // We dont want multiple checkpoints messing with each other
        if (CheckpointData.returnCheckpoint().location != playerLocation)
        {
            return;
        }


        if (!CheckpointData.returnCheckpoint().camerafollow && !stopMovingTheCamera) 
        {
            CameraFollowStaticCamera();
        }    


        if (CheckpointData.returnCheckpoint().camerafollow)
        {            
            CameraFollowDynamicCamera();
        }


        if (Camera.main.orthographicSize != cameraSize) 
        {
            CameraSizeIncreaseDecrease();
        }
    }


    void CameraFollowStaticCamera() {
        // Makes the camera snap to the target position, kind of like in Celeste

        Vector3 currentPosition = Camera.main.transform.position;
        Vector3 targetPosition = new Vector3(cameraPosition.x, cameraPosition.y, -10f);
        float step = cameraSpeed * Time.deltaTime;

        Vector3 position = Vector3.MoveTowards(
            currentPosition,
            targetPosition,
            step
            );
    
        Camera.main.transform.position = position;

        // Deze logica werkt niet, heb het dus uit gezet
        // cameraSpeed = cameraSpeed * cameraSpeedIncrease * Time.deltaTime;
    
        if ( (Vector2) Camera.main.transform.position == cameraPosition)
        {
            print("STopping camera follow!!");
            stopMovingTheCamera=true;
            // cameraSpeed = initialCameraSpeed;
        }
    }

    void CameraFollowDynamicCamera() {
        Vector3 newPosition = Vector3.MoveTowards(
        Camera.main.transform.position
        ,new Vector3(
            player.transform.localPosition.x + cameraDistanceFromPlayer
            , player.transform.localPosition.y+2.4f 
            , -10f) 
        , cameraSpeed * Time.deltaTime);
        Camera.main.transform.position = newPosition;
        cameraSpeed *= cameraSpeedIncrease;
    }


    void CameraSizeIncreaseDecrease() {
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
