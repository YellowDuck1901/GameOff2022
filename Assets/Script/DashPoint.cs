using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPoint : MonoBehaviour
{
    PlayerMovement Player;

    SpriteRenderer sr;

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        sr = Player.GetComponent<SpriteRenderer>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if(Player._dashesLeft == 0)
            {
                sr.color = Color.white;
                Player._dashesLeft++;
                gameObject.SetActive(false);
            }
        }
    }


}
