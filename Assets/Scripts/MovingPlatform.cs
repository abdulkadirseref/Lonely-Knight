using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> wayPoints;
    public float moveSpeed;
    public int target;
 
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[target].position, moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (transform.position == wayPoints[target].position)
        {
            if (target == wayPoints.Count - 1)
            {
                target = 0;
            }
            else
            {
                target += 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);         
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
