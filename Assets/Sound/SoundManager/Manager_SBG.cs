using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Manager_SBG : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Game1")]
    public AudioClip backgroundG1;

    [Header("Game2")]
    public AudioClip backgroundG2;

    [Header("Game3")]
    public AudioClip backgroundG3;

    public static Manager_SBG instance;

    AudioSource audio;
    static float volumeDefault_SBG;
    static float pitchDefault_SBG;
    static int priorityDefault_SBG;
    // Use this for initialization
    void Start()
    {
        audio = gameObject.GetComponent<AudioSource>();
        instance = this;
    }
    public static void PlaySound(soundsGame currentSound)
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.loop = true;

        switch (currentSound)
        {

            case soundsGame.backgroundG1:
                {
                    audioSource.clip = instance.backgroundG1;
                    audioSource.Play();
                }
                break;

            case soundsGame.backgroundG2:
                {
                    audioSource.clip = instance.backgroundG2;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                break;

            case soundsGame.backgroundG3:
                {
                    audioSource.clip = instance.backgroundG3;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                break;
        }
    }

    public static void PlaySound(soundsGame currentSound, float volume, float pitch, int priority = 128)
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.priority = priority;
        switch (currentSound)
        {
            case soundsGame.backgroundG1:
                {
                    audioSource.clip = instance.backgroundG1;
                    audioSource.Play();
                    resetSettingSBG();
                }
                break;

            case soundsGame.backgroundG2:
                {
                    audioSource.clip = instance.backgroundG2;
                    audioSource.loop = true;
                    audioSource.Play();
                    resetSettingSBG();
                }
                break;

            case soundsGame.backgroundG3:
                {
                    audioSource.clip = instance.backgroundG3;
                    audioSource.loop = true;
                    audioSource.Play();
                    resetSettingSBG();
                }
                break;
        }
    }

    public static void resetSettingSBG()
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.priority = priorityDefault_SBG;
        audioSource.volume = volumeDefault_SBG;
        audioSource.pitch = pitchDefault_SBG;
    }

    public static void stopPlay()
    {
        var audioSource = instance.GetComponent<AudioSource>();
        audioSource.Stop();
    }
}

