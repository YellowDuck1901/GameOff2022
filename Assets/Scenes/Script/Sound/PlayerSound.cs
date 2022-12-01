using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    // Start is called before the first frame update
   public void playSoundRun()
    {
        Manager_SFX.PlaySound_SFX(soundsGame.Run,0.3f,1,50);
    }

    public void playSoundSlide()
    {
        Manager_SFX.PlaySound_SFX(soundsGame.Slice, 1f, 0, 128);
    }

    public void playSoundDash()
    {
        Manager_SFX.PlaySound_SFX(soundsGame.Dash,0.3f,3,128);
    }

    public void playSoundJump()
    {
        Manager_SFX.PlaySound_SFX(soundsGame.Jump,0.5f,3f,128);
    }

    public void playSoundLanding()
    {
        Manager_SFX.PlaySound_SFX(soundsGame.Landing, 1f, 1f, 128);
    }

    public void playSoundDead()
    {
        Debug.Log("PlaySounDead");
        Manager_SFX.PlaySound_SFX(soundsGame.Dead);
    }
}
