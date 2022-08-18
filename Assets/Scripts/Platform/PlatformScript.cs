using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float movementSpeed;

    Directions direction;
    public int defaultDirection = 1;

    private List<GameObject> objectsToMoveAlong = new List<GameObject>();
    private Vector2 previousLocation;

    enum Directions
    {
        left,
        right
    }

    private void Start()
    {
        if (defaultDirection == 1)
        {
            direction = Directions.right;
        }
        else
        {
            direction = Directions.left;
        }

        previousLocation = transform.position;
    }

    void Update()
    {
        if (direction == Directions.right)
        {
            Vector2 nextMove = Vector2.MoveTowards(
                transform.position,
                new Vector2(maxX, transform.position.y),
                movementSpeed * Time.deltaTime
                );

            transform.position = nextMove;

            if (transform.position.x == maxX)
            {
                direction = Directions.left;
            }
        }
        
        else if (direction == Directions.left)
        {
            Vector2 nextMove = Vector2.MoveTowards(
            transform.position,
            new Vector2(minX, transform.position.y),
            movementSpeed * Time.deltaTime
            );

            transform.position = nextMove;

            if (transform.position.x == minX)
            {
                direction = Directions.right;
            }
        }


        // Move along all objects on platform
        float deltaX = transform.position.x - previousLocation.x;
        
        foreach (GameObject i in objectsToMoveAlong)
        {
            Vector2 temp = i.transform.position;
            temp.x += deltaX;
            i.transform.position = temp;
        }
        previousLocation = transform.position; 


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        objectsToMoveAlong.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        objectsToMoveAlong.Remove(collision.gameObject);
    }

}
