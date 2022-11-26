using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class PenatlyManager : MonoBehaviour
{
    public static bool Penatly;

    private void Update()
    {
        if (StatusPlayer.playerInstance.IsHit)
        {
            Penatly = false;
        }

    }
}
