using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LampSwitch : MonoBehaviour
{
    [SerializeField]
    private int triggerId;

    [SerializeField]
    private bool turnOn;

    [SerializeField]
    private float duration;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (turnOn) EventManager.LighUpLamp(triggerId, duration);
            else EventManager.FadeLamp(triggerId, duration);
        }
    }
}
