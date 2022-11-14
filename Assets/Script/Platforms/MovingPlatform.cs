using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private List<Transform> wayPoints;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int target;

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[target].position, moveSpeed * Time.fixedDeltaTime);
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


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlatformSpeed"))
        {
            moveSpeed = Int32.Parse(collision.gameObject.name);
        }
    }
    void OnDrawGizmosSelected()
    {


        for (int i = 0; i < wayPoints.Count - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
            wayPoints[i].position,
            wayPoints[i + 1].position);


        }

        Gizmos.DrawLine(
           wayPoints[0].position,
           wayPoints[wayPoints.Count-1].position);


        //Gizmos.DrawWireSphere(Vector3.zero, 0.05f);

        //Gizmos.color = Color.white;
    }

}
