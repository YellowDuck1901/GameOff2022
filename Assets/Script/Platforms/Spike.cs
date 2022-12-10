using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SearchService;
using UnityEditor.Sequences;
#endif
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
            sp.IsHit = true;
        }
    }
}
