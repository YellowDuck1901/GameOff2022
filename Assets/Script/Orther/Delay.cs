using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay : MonoBehaviour
{
    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void delayTime(float time)
    {
        StartCoroutine(delay(time));
    }
}
