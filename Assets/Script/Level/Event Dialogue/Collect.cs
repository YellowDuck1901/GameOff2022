using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class Collect : MonoBehaviour
{
    [HideInInspector]
    public bool isCollision;

    public static bool isCollectDialogue;

    PlayerMovement player;

    SpriteRenderer sr;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && sr.enabled)
        {
            isCollision = true;
        }
    }

    private void Update()
    {
        if(sr.enabled && isCollision && player._isGrounded)
        {
            sr.enabled = false;
            isCollectDialogue = true;
        }
    }
}   
