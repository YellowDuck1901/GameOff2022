using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_Off_After_Switch : MonoBehaviour
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
            if (turnOn) EventManager.TurnOnLampAfter(triggerId,duration);
            else EventManager.TurnOffLampAfter(triggerId,duration);
        }
    }
}
