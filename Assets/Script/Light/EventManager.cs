using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action<int> TurnOnLampEvent;

    public static event Action<int> TurnOffLampEvent;

    public static event Action<int,float> FadeLampEvent;

    public static event Action<int,float> LighUpLampEvent;

    public static event Action<int, float> TurnOnLampAfterEvent;

    public static event Action<int, float> TurnOffLampAfterEvent;

    public static void TurnOnLamp(int id)
    {
        TurnOnLampEvent?.Invoke(id);
    }

    public static void TurnOffLamp(int id)
    {
        TurnOffLampEvent?.Invoke(id);
    }

    public static void FadeLamp(int id,float duration)
    {
        FadeLampEvent?.Invoke(id,duration);
    }

    public static void LighUpLamp(int id,float duration)
    {
        LighUpLampEvent?.Invoke(id,duration);
    }

    public static void TurnOnLampAfter(int id, float duration)
    {
        TurnOnLampAfterEvent?.Invoke(id, duration);
    }

    public static void TurnOffLampAfter(int id, float duration)
    {
        TurnOffLampAfterEvent?.Invoke(id, duration);
    }
}
