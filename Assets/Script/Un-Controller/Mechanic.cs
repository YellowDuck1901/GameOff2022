using Ink.Runtime;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Mechanic : MonoBehaviour
{
    public enum Movements
    {
        MoveLeft,
        MoveRight,
        FaceLeft,
        FaceRight,
        Dash,
        Jump,
        WallJump,
        Slide,
        SlideUp,
        SlideDown
    }

    List<KeyCode> listKey = new List<KeyCode>() { KeyCode.A,KeyCode.LeftArrow, KeyCode.D, KeyCode.RightArrow, KeyCode.C,KeyCode.X,KeyCode.Z,KeyCode.Space};
    static bool isLongPress_Jump, isLongPress_slide, isLongPress_Left, isLongPress_Right, isLongPress_Dash;
    public static int CountMoveLeft, CountMoveRight, CountDash,
        CountJump, CountWallJump, Countslide, CountslideUp, CountslideDown;
    bool deadInRangeTime;
    bool deadLitmitAciton;

    public int GetCountMovement(Movements moveType)
    {
        int count = 0;
        switch (moveType)
        {
            case Movements.MoveLeft:
                count = CountMoveLeft;
               
                break;
            case Movements.MoveRight:
                count = CountMoveRight;
                break;

            case Movements.Dash:
                count = CountDash;
                break;

            case Movements.Jump:
                count = CountJump;
                break;

            case Movements.WallJump:
                count = CountWallJump;
                break;

            case Movements.Slide:
                count = Countslide;
                break;

            case Movements.SlideUp:
                count = CountslideUp;
                break;

            case Movements.SlideDown:
                count = CountslideDown;
                break;
        }
        return count;
    }

    public int SetCountMovement(Movements moveType, int value)
    {
        int count = 0;
        switch (moveType)
        {
            case Movements.MoveLeft:
               CountMoveLeft = value;

                break;
            case Movements.MoveRight:
               CountMoveRight = value;
                break;

            case Movements.Dash:
               CountDash = value;
                break;

            case Movements.Jump:
                CountJump = value;
                break;

            case Movements.WallJump:
                CountWallJump = value;
                break;

            case Movements.Slide:
                Countslide = value;
                break;

            case Movements.SlideUp:
                 CountslideUp = value;
                break;

            case Movements.SlideDown:
               CountslideDown = value;
                break;
        }
        return count;
    }

    //public void SetCountMovement(Movements moveType)
    //{
    //    switch (moveType)
    //    {
    //        case Movements.MoveLeft:
    //            CountMoveLeft = 0;
    //            break;
    //        case Movements.MoveRight:
    //            CountMoveRight = 0;
    //            break;
    //        case Movements.Dash:
    //            CountDash = 0;
    //            break;
    //        case Movements.Jump:
    //            CountJump = 0;
    //            break;
    //        case Movements.WallJump:
    //            CountWallJump = 0;
    //            break;
    //        case Movements.slide:
    //            Countslide = 0;
    //            break;
    //        case Movements.slideUp:
    //            CountslideUp = 0;
    //            break;
    //        case Movements.slideDown:
    //            CountslideDown = 0;
    //            break;
    //    }
    //}
    public bool GetBoolLong_Press(Movements moveType)
    {
        switch (moveType)
        {
            case Movements.MoveLeft:
                Debug.Log("MoveLeft");

                return isLongPress_Left;

            case Movements.MoveRight:
                Debug.Log("MoveRight");

                return isLongPress_Right;

            case Movements.Dash:
                Debug.Log("Dash");

                return isLongPress_Dash;

            case Movements.Jump:
                Debug.Log("Jump " + isLongPress_Jump);
                return isLongPress_Jump;

            case Movements.Slide:
                Debug.Log("Slide");

                return isLongPress_slide;
        }
        return false;
    }

    public void SetBoolLong_Press(Movements moveType, bool value)
    {
        switch (moveType)
        {
            case Movements.MoveLeft:
                isLongPress_Left = value;
                break;
            case Movements.MoveRight:
                isLongPress_Right = value;
                break;

            case Movements.Dash:
                isLongPress_Dash = value;
                break;

            case Movements.Jump:
                isLongPress_Jump = value;
                break;

            case Movements.Slide:
                isLongPress_slide = value;
                break;
        }
    }

    public void SetBoolLong_Press(KeyCode keyCode, bool value)
    {
        switch (keyCode)
        {
            case KeyCode.A:
            case KeyCode.LeftArrow:
                isLongPress_Left = value;
                break;

            case KeyCode.D:
            case KeyCode.RightArrow:
                isLongPress_Right = value;
                break;

            case KeyCode.Space:
            case KeyCode.C:
                isLongPress_Jump = value;
                break;

            case KeyCode.X:
                isLongPress_Dash = value;
                break;

            case KeyCode.Z:
                isLongPress_slide = value;
                break;
        }
    }
    public List<KeyCode> GetkeyMovement(Movements moveType)
    {
        switch (moveType)
        {
            case Movements.MoveLeft:
                return new List<KeyCode> { KeyCode.A,KeyCode.LeftArrow};

            case Movements.MoveRight:
                return new List<KeyCode> { KeyCode.D,KeyCode.RightArrow};

            case Movements.Dash:
                return new List<KeyCode> { KeyCode.X,KeyCode.LeftShift};

            case Movements.Jump:
                return new List<KeyCode> { KeyCode.Space,KeyCode.C};

            case Movements.Slide:
                return new List<KeyCode> { KeyCode.Z};

            //case Movements.slideUp:
            //    return CountslideUp;

            //case Movements.slideDown:
            //    return CountslideDown;
        }
        return null;
    }

    public  IEnumerator limitNumberMovement(Movements moveType, int limitNumber)
    {
        int Oddcount = GetCountMovement(moveType);
        while (true)
        {
            if (StatusPlayer.playerInstance.IsHit)
            {
                SetCountMovement(moveType, 0);
                Oddcount = 0;
                break;
            }

            if (GetCountMovement(moveType) - Oddcount >= limitNumber)
            {
                Debug.Log("Penalty " + moveType);
                PenatlyManager.Penatly = true;

                break;
            }
            yield return null;
        }
    }

    public IEnumerator limitAndDisableNumberMovement(Movements moveType, int limitNumber)
    {
        Debug.Log("limitAndDisableNumberMovement");
        int Oddcount = GetCountMovement(moveType);
        while (true)
        {
            if (StatusPlayer.playerInstance.IsHit)
            {
                SetCountMovement(moveType, 0);
                Oddcount = 0;
                break;
            }

            if (GetCountMovement(moveType) - Oddcount >= limitNumber)
            {
                setDisableMovement(moveType, true);
                Debug.Log("Penalty " + moveType);
                PenatlyManager.Penatly = true;

                break;
            }
            yield return null;
        }
    }

    public IEnumerator limitTimePressMovement(Movements moveType, float limitNumber)
    {
        while (true)
        {

            if (GetBoolLong_Press(moveType))
            {
                int timer = 0;
                for (int i = 0; i < limitNumber; i++)
                {
                    if (GetBoolLong_Press(moveType))
                    {
                        timer++;
                        yield return new WaitForSeconds(1);
                    }
                    else break;
                }

                if (timer >= limitNumber)
                {
                    timer = 0;
                    Debug.Log("Penalty " + moveType);
                    PenatlyManager.Penatly = true;

                    break;
                }
            }
            yield return null;


        }
    }

    public IEnumerator doActionInTimeRange(Movements moveType, int numberMovement, float limitTime)
    {
        SetCountMovement(moveType, 0);
        yield return new WaitForSeconds(limitTime);


        if (deadInRangeTime)
        {
            deadInRangeTime = false;
            yield return null;
        }
        int counter = GetCountMovement(moveType);
        if (counter >= numberMovement)
        {
            Debug.Log("Penalty " + moveType);
            PenatlyManager.Penatly = true;
        }
        else 
        {
            Debug.Log("Not Penalty " + moveType);
        }
    }
    
    public void setDisableMovement(Movements moveType,bool value)
    {
        PenatlyManager.Penatly = true;
        switch (moveType)
        {
            case Movements.MoveLeft:
                PlayerMovement._disableLeft = value;
                break;
            case Movements.MoveRight:
                PlayerMovement._disableRight = value;
                break;
            case Movements.Dash:
                PlayerMovement._disableDash = value;
                break;
            case Movements.Jump:
                PlayerMovement._disableJump = value;
                break;
            case Movements.Slide:
                PlayerMovement._disableslide = value;
                break;
            case Movements.SlideDown:
                PlayerMovement._disableslideDown = value;
                break;
            case Movements.SlideUp:
                PlayerMovement._disableslideUp = value;
                break;
            case Movements.FaceLeft:
                PlayerMovement._disableFaceLeft = value;
                break;
            case Movements.FaceRight:
                PlayerMovement._disableFaceRight = value;
                break;
        }
    }

    public static void resetDisableMovement()
    {
        PlayerMovement._disableLeft = false;
        PlayerMovement._disableRight = false;
        PlayerMovement._disableDash = false;
        PlayerMovement._disableJump = false;
        PlayerMovement._disableslide = false;
        PlayerMovement._disableslideDown = false;
        PlayerMovement._disableslideUp = false;
    }

    private void Update()
    {
        for (int i = 0; i < listKey.Count; i++)
        {
            if (Input.GetKeyDown(listKey[i]))
            {
                SetBoolLong_Press(listKey[i], true);
            }

            if (Input.GetKeyUp(listKey[i]))
            {
                SetBoolLong_Press(listKey[i], false);
            }


            if (StatusPlayer.playerInstance.IsHit)
            {
                deadInRangeTime = true;

            }
        }

    }

   
}
