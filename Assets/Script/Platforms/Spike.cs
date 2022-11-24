using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEditor.Sequences;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    public LoadScene LoadLevel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            StatusPlayer sp = collision.gameObject.GetComponent<StatusPlayer>();
            LoadLevel.openSceneWithColdDown(SceneManager.GetActiveScene().name,0f);

            // reset penatly
            PenatlyManager.Penatly = false; 
            //sp.IsDead = true;
        }
    }
}
