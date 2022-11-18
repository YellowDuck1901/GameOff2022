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
        Dash,
        Jump,
        WallJump,
        Slide,
        SlideUp,
        SlideDown
    }

    List<KeyCode> listKey = new List<KeyCode>() { KeyCode.A,KeyCode.LeftArrow, KeyCode.D, KeyCode.RightArrow, KeyCode.C,KeyCode.X,KeyCode.Z,KeyCode.Space};
    bool isPenatly;
    bool isLongPress_Jump, isLongPress_slide, isLongPress_Left, isLongPress_Right, isLongPress_Dash;
    public static int CountMoveLeft, CountMoveRight, CountDash,
        CountJump, CountWallJump, Countslide, CountslideUp, CountslideDown;
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
                return isLongPress_Left;

            case Movements.MoveRight:
                return isLongPress_Right;

            case Movements.Dash:
                return isLongPress_Dash;

            case Movements.Jump:
                return isLongPress_Jump;

            case Movements.Slide:
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
            if (GetCountMovement(moveType) - Oddcount >= limitNumber)
            {
                Debug.Log("Trigger Penalty "+moveType);
                isPenatly = true;
                break;
            }
            yield return new WaitForEndOfFrame();
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
                    Debug.Log("trigger " + moveType);
                    isPenatly = true;
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator doActionInTimeRange(Movements moveType, int numberMovement, float limitTime)
    {
        yield return new WaitForSeconds(limitTime);
        int counter = GetCountMovement(moveType);
        SetCountMovement(moveType, 0);
        if (counter >= numberMovement)
        {
            isPenatly = true;
            Debug.Log("fail");
        }
        else 
        {
            isPenatly = false;
            Debug.Log("win");
        }
    }
    
    public void setDisableMovement(Movements moveType,bool value)
    {
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
        }
    }

    public void resetDisableMovement()
    {
        PlayerMovement._disableLeft = false;
        PlayerMovement._disableRight = false;
        PlayerMovement._disableDash = false;
        PlayerMovement._disableJump = false;
        PlayerMovement._disableslide = false;
        PlayerMovement._disableslideDown = false;
        PlayerMovement._disableslideUp = false;
    }

    private void Start()
    {
        //StartCoroutine(limitNumberMovement(Movements.Jump, 3));
        //StartCoroutine(limitNumberMovement(Movements.Dash, 3));
        //StartCoroutine(limitNumberMovement(Movements.MoveRight, 3));
        //StartCoroutine(limitNumberMovement(Movements.MoveLeft, 3));
        //StartCoroutine(limitNumberMovement(Movements.slide, 3));
        //StartCoroutine(limitNumberMovement(Movements.SlideUp, 3));
        //StartCoroutine(limitNumberMovement(Movements.SlideDown, 3));

        //StartCoroutine(limitTimePressMovement(Movements.Dash, 3f));
        //StartCoroutine(limitTimePressMovement(Movements.MoveLeft, 3f));
        //StartCoroutine(limitTimePressMovement(Movements.MoveRight, 3f));
        //StartCoroutine(limitTimePressMovement(Movements.Slide, 3f));

        //StartCoroutine(doActionInTimeRange(Movements.Jump, 3, 4));
        //StartCoroutine(doActionInTimeRange(Movements.Slide, 3, 4));

        //setDisableMovement(Movements.Jump, true);
    }

    private void Update()
    {   
        for (int i = 0; i < listKey.Count; i++)
        {
            if (Input.GetKeyDown(listKey[i]))
            {
                SetBoolLong_Press( listKey[i], true);
            }

            if (Input.GetKeyUp(listKey[i]))
            {
                SetBoolLong_Press(listKey[i], false);
            }
        }
    }
}
