using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject player;
    public int level;
    public Vector2 startPosition;
    public bool cameraFollow;
    public Vector2 cameraPosition;
    public bool reversedGravity;

    private void Awake() 
    {
        // This should execute only once every scene.
        CheckpointData CPdata = CheckpointData.returnCheckpoint(); 
        if (CPdata == null)
        {
            CheckpointData checkpointData = new CheckpointData();
            checkpointData.camerafollow=cameraFollow;
            checkpointData.level = level;
            checkpointData.location = startPosition;
            checkpointData.cameraPosition = new Vector3(cameraPosition.x, cameraPosition.y, -10);
            checkpointData.reversedGravity = reversedGravity;
            CheckpointData.SetCheckpoint(checkpointData);
        }

        else 
        {
            // So this'll only be usefull if we are changing scenes
            // Each scene will be a seperate world
            // I don't think this is the most optimal way of checking
            // Actually we could just scrap this altogether but I'm not very sure yet

            // Update about 6 months later: What the fuck????
            if (CPdata.level != level)
            {
                // I could be onto somethig here
                // This should probably change the scene based on the level thohgh
                // instead of whatever the fuck its doing here 
                CheckpointData checkpointData = new CheckpointData();
                checkpointData.camerafollow=cameraFollow;
                checkpointData.level = level;
                checkpointData.location = startPosition;
                checkpointData.cameraPosition = new Vector3(cameraPosition.x, cameraPosition.y, -10);
                checkpointData.reversedGravity = false;
                CheckpointData.SetCheckpoint(checkpointData);
            }
        }

        player.transform.position = CheckpointData.returnCheckpoint().location;
        Camera.main.transform.position = CheckpointData.returnCheckpoint().cameraPosition;


        if (CheckpointData.returnCheckpoint().reversedGravity) 
        {
            Rotater.ReverseGravity(player);
        }


    }



    
}
