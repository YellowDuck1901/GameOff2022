using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Off_Switch : MonoBehaviour
{
    [SerializeField]
    private int triggerId;

    [SerializeField]
    private bool turnOn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (turnOn) EventManager.TurnOffLamp(triggerId);
            else EventManager.TurnOffLamp(triggerId);
        }
    }
}
