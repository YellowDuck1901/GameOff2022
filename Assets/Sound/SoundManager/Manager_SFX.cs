using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_SFX : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Movemoment")]
    public AudioClip Run;
    public AudioClip Jump;
    public AudioClip Landing;
    public AudioClip Dash;
    public AudioClip Slice;
    public AudioClip Dead;


    [Header("Platform")]
    public AudioClip Move;
    public AudioClip Falling;
   

    [Header("Orther")]
    public AudioClip Collect;
   
    public static Manager_SFX instance;
    AudioSource audio;
    static float volumeDefault_SFX;
    static float pitchDefault_SFX;
    static int priorityDefault_SFX;
    // Use this for initialization
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        volumeDefault_SFX = audio.volume;
        pitchDefault_SFX = audio.pitch;
        priorityDefault_SFX = audio.priority;
        instance = this;

        Debug.Log("volumeDefault_SFX " + volumeDefault_SFX);
        Debug.Log("pitchDefault_SFX " + pitchDefault_SFX);
        Debug.Log("priorityDefault_SFX " + priorityDefault_SFX);

    }
    public static void PlaySound_SFX(soundsGame currentSound)
    {
        resetSettingSFX();
        switch (currentSound)
        {
            case soundsGame.Run:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Run);
                }
                break;
            case soundsGame.Jump:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Jump);
                }
                break;
            case soundsGame.Landing:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Landing);
                }
                break;
            case soundsGame.Dash:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Dash);
                }
                break;

            case soundsGame.Slice:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Slice);
                }
                break;
            case soundsGame.Dead:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Dead);
                }
                break;
            case soundsGame.Collect:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Collect);
                }
                break;

            case soundsGame.Falling:
                {
                    instance.GetComponent<AudioSource>().PlayOneShot(instance.Falling);
                }
                break;

        }
    }

    public static void PlaySound_SFX(soundsGame currentSound, float volume, float pitch, int priority = 128)
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.priority = priority;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        switch (currentSound)
        {
            case soundsGame.Run:
                {
                    audioSource.clip = instance.Run;
                    audioSource.Play();
                    //resetSettingSFX();
                }
                break;
            case soundsGame.Jump:
                {
                    audioSource.clip = instance.Jump;
                    audioSource.Play();
                    //resetSettingSFX();
                }
                break;
            case soundsGame.Landing:
                {
                    audioSource.clip = instance.Landing;
                    audioSource.Play();
                }
                break;
            case soundsGame.Dash:
                {
                    audioSource.clip = instance.Dash;
                    audioSource.Play();
                    //resetSettingSFX();
                }
                break;

            case soundsGame.Slice:
                {
                    audioSource.clip = instance.Slice;
                    audioSource.Play();
                    //resetSettingSFX();
                }
                break;
            case soundsGame.Dead:
                {
                    audioSource.clip = instance.Dead;
                    audioSource.Play();
                    //resetSettingSFX();
                }
                break;
            case soundsGame.Collect:
                {
                    audioSource.clip = instance.Dead;
                    audioSource.Play();
                }
                break;
            case soundsGame.Falling:
                {
                    audioSource.clip = instance.Falling;
                    audioSource.Play();
                }
                break;

        }
    }

    public static void resetSettingSFX()
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.priority = priorityDefault_SFX;
        audioSource.volume = volumeDefault_SFX;
        audioSource.pitch = pitchDefault_SFX;
    }

    public static void stopPlay()
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.Stop();
    }
}
