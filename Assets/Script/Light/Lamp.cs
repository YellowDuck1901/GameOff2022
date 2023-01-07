using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    [SerializeField]
    private int lampId;

    private Light2D light2D;

    #region Atrribute Fade, light up lamp

    private bool isOperating; //true when lamp is fading or getting brighter
    #endregion


    #region Data properties Light2D
    private float OuterLight;

    private float InnerLight;

    private float Intensity;
    #endregion


    void Start()
    {
        light2D = gameObject.GetComponent<Light2D>();
        EventManager.TurnOnLampEvent += TurnOnLamp;
        EventManager.TurnOffLampEvent += TurnOffLamp;
        EventManager.FadeLampEvent += TurnOnFadeLamp;
        EventManager.LighUpLampEvent += TurnOnLightUpLamp;
        EventManager.TurnOffLampAfterEvent += TurnOffLampAfter;
        EventManager.TurnOnLampAfterEvent += TurnOnLampAfter;


        OuterLight = light2D.pointLightOuterRadius;
        InnerLight = light2D.pointLightInnerRadius;
        Intensity = light2D.intensity;
    }

    // Update is called once per frame

    #region Turn On/Off Lamp
    private void TurnOnLamp(int triggerId)
    {
        if(triggerId == lampId)
        {
            light2D.intensity = Intensity;
        }
    }

    private void TurnOffLamp(int triggerId)
    {
        if (triggerId == lampId)
        {
            light2D.intensity = 0;
        }
    }

    private void TurnOffLampAfter(int triggerId, float time)
    {
        if (triggerId == lampId)
        {
            StartCoroutine(IE_TurnOffLampAfter(triggerId,time));
        }
    }

    IEnumerator IE_TurnOffLampAfter(int triggerId, float time)
    {
        yield return new WaitForSeconds(time);
        TurnOffLamp(triggerId);
    }
    private void TurnOnLampAfter(int triggerId, float time)
    {
        if (triggerId == lampId)
        {
            StartCoroutine(IE_TurnOnLampAfter(triggerId, time));
        }
    }

    IEnumerator IE_TurnOnLampAfter(int triggerId, float time)
    {
        yield return new WaitForSeconds(time);
        TurnOnLamp(triggerId);
    }
    #endregion

    #region Fade/Light up lamp
    private void TurnOnFadeLamp(int triggerId, float duration)
    {
        if (triggerId == lampId)
        {
            if (!isOperating && light2D.pointLightOuterRadius != InnerLight)
                StartCoroutine(FadeLamp(duration));
        }
    }

    IEnumerator FadeLamp( float duration)
    {
        float rate = 0;
        float time = 0;
        isOperating = true;

        while (true)
        {
            rate = 1 / duration;
            time += rate * Time.deltaTime;
            light2D.pointLightOuterRadius = Mathf.Lerp(OuterLight, InnerLight, time);
            yield return new WaitForFixedUpdate();

            if (light2D.pointLightOuterRadius == InnerLight) 
            {
                isOperating = false;
                break;
            }
        }
    }


    private void TurnOnLightUpLamp(int triggerId, float duration)
    {
        if (triggerId == lampId)
        {
            if (!isOperating && light2D.pointLightOuterRadius != OuterLight)
                StartCoroutine(LightUpLamp(duration));
        }
    }

    IEnumerator LightUpLamp(float duration)
    {
        float rate = 0;
        float time = 0;
        isOperating = true;

        while (true)
        {
            rate = 1 / duration;
            time += rate * Time.deltaTime;
            light2D.pointLightOuterRadius = Mathf.Lerp(InnerLight, OuterLight, time);
            yield return new WaitForFixedUpdate();

            if (light2D.pointLightOuterRadius == OuterLight)
            {
                isOperating = false;
                break;
            }
        }
    }

    #endregion


}
