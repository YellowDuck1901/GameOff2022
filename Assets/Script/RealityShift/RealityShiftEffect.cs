using Kino;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityShiftEffect : MonoBehaviour
{
    public DigitalGlitch GlitchEffect;
    public float Intensity;

    public AnalogGlitch GlitchEffect2;
    public float ScanLineJitter;
    public float HorizontalShake;
    public float ColorDrift;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Vector3 distanceVector = collision.transform.position - transform.position;
            /*GlitchEffect.intensity = Intensity / distanceVector.magnitude;*/
            GlitchEffect2.scanLineJitter = ScanLineJitter / distanceVector.magnitude;
            GlitchEffect2.horizontalShake = HorizontalShake / distanceVector.magnitude;
            GlitchEffect2.colorDrift = ColorDrift / distanceVector.magnitude;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            /*GlitchEffect.intensity = 0f;*/
            GlitchEffect2.scanLineJitter = 0f;
            GlitchEffect2.horizontalShake = 0f;
            GlitchEffect2.colorDrift = 0f;
        }
    }
}
