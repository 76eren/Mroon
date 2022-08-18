using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointData 
{
    public Vector2 location;
    public int level;
    public Vector3 cameraPosition;

    public bool camerafollow;
    public bool reversedGravity; 



    public static void SetCheckpoint(CheckpointData newCheckpoint)
    {
        string json = JsonUtility.ToJson(newCheckpoint);
        PlayerPrefs.SetString("checkpoint", json);
    }

    public static CheckpointData returnCheckpoint()
    {
        return JsonUtility.FromJson<CheckpointData>(PlayerPrefs.GetString("checkpoint"));
        
    }
}

