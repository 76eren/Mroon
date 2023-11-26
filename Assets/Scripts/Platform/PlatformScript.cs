using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float movementSpeed;

    private Directions direction;
    [SerializeField] private int defaultDirection = 1;
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
    Vector2 targetPosition = direction == Directions.right ? 
                            new Vector2(maxX, transform.position.y) : 
                            new Vector2(minX, transform.position.y);

    MoveTowardsTarget(targetPosition);

    if ((direction == Directions.right && transform.position.x == maxX) ||
        (direction == Directions.left && transform.position.x == minX))
    {
        direction = direction == Directions.right ? Directions.left : Directions.right;
    }

    MoveObjectsOnPlatform();
}

void MoveTowardsTarget(Vector2 target)
{
    Vector2 nextMove = Vector2.MoveTowards(
        transform.position,
        target,
        movementSpeed * Time.deltaTime
    );

    transform.position = nextMove;
}

void MoveObjectsOnPlatform()
{
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
