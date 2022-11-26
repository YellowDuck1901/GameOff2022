using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEditor.Sequences;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            StatusPlayer sp = collision.gameObject.GetComponent<StatusPlayer>();
            // reset penaty
            PenatlyManager.Penatly = false; 
            sp.IsHit = true;
        }
    }
}
