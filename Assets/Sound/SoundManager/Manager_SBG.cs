using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Manager_SBG : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Menu")]
    public AudioClip Menu;

    [Header("MusicBackGround")]
    public AudioClip MusicBackGround;

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

        if (!audio.isPlaying)
            Manager_SBG.PlaySound(soundsGame.BackGround);
    }
    public static void PlaySound(soundsGame currentSound)
    {
        if (!Manager_SBG.instance.audio.isPlaying) {
            var audioSource = instance.GetComponent<AudioSource>();
            audioSource.loop = true;

            switch (currentSound)
            {

                case soundsGame.Menu:
                    {
                        audioSource.clip = instance.Menu;
                        audioSource.Play();
                    }
                    break;

                case soundsGame.BackGround:
                    {
                        audioSource.clip = instance.MusicBackGround;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    break;
            }
        }
    }

    public static void PlaySound(soundsGame currentSound, float volume, float pitch, int priority = 128)
    {
        if (!Manager_SBG.instance.audio.isPlaying)
        {
            var audioSource = instance.GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.priority = priority;
            switch (currentSound)
            {
                case soundsGame.Menu:
                    {
                        audioSource.clip = instance.Menu;
                        audioSource.Play();
                        resetSettingSBG();
                    }
                    break;

                case soundsGame.BackGround:
                    {
                        audioSource.clip = instance.MusicBackGround;
                        audioSource.loop = true;
                        audioSource.Play();
                        resetSettingSBG();
                    }
                    break;
            }
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

