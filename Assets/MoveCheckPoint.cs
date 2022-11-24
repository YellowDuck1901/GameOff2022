using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCheckPoint : MonoBehaviour
{
    Transform startPosition;
    private void Start()
    {
        startPosition = GameObject.Find("StartPosition").GetComponent<Transform>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            startPosition.position = gameObject.transform.position;
            Destroy(gameObject);
        }
    }
}
