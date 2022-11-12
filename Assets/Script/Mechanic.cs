using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        Slice,
        SliceUp,
        SliceDown
    }

    List<KeyCode> listKey;

    bool isLongPress;
    public static int CountMoveLeft, CountMoveRight, CountDash,
        CountJump, CountWallJump, CountSlide, CountSliceUp, CountSlideDown;
    public int GetCountMovement(Movements moveType)
    {
        switch (moveType)
        {
            case Movements.MoveLeft:
                return CountMoveLeft;

            case Movements.MoveRight:
                return CountMoveRight;

            case Movements.Dash:
                return CountDash;

            case Movements.Jump:
                return CountJump;

            case Movements.WallJump:
                return CountWallJump;

            case Movements.Slice:
                return CountSlide;

            case Movements.SliceUp:
                return CountSliceUp;

            case Movements.SliceDown:
                return CountSlideDown;
        }
        return 0;
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

            case Movements.Slice:
                return new List<KeyCode> { KeyCode.Z};

            //case Movements.SliceUp:
            //    return CountSliceUp;

            //case Movements.SliceDown:
            //    return CountSlideDown;
        }
        return null;
    }

    IEnumerator limitNumberMovement(Movements moveType, int limitNumber)
    {
        int Oddcount = GetCountMovement(moveType);
        while (true)
        {
            if (GetCountMovement(moveType) - Oddcount >= limitNumber)
            {
                Debug.Log("Trigger Penalty "+moveType);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator limitTimePressMovement(Movements moveType, float limitNumber)
    {
        listKey = GetkeyMovement(moveType);
        while (true)
        {
            if (isLongPress)
            {
                yield return new WaitForSeconds(limitNumber);
                if(isLongPress)
                {
                    Debug.Log("trigger");
                    break;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public bool getButtonDownWithKey(KeyCode keycode)
    {
        return Input.GetKeyDown(keycode);
    }
    private void Start()
    {
        //StartCoroutine(limitNumberMovement(Movements.Jump, 3));
        //StartCoroutine(limitNumberMovement(Movements.Dash, 3));
        //StartCoroutine(limitNumberMovement(Movements.MoveRight, 3));
        //StartCoroutine(limitNumberMovement(Movements.MoveLeft, 3));
        //StartCoroutine(limitNumberMovement(Movements.Slice, 3));
        //StartCoroutine(limitNumberMovement(Movements.SliceDown, 3));
        //StartCoroutine(limitNumberMovement(Movements.SliceUp, 3));

        StartCoroutine(limitTimePressMovement(Movements.Jump, 2f));
    }

    private void Update()
    {   
        listKey = GetkeyMovement(Movements.Jump);
        for (int i = 0; i < listKey.Count; i++)
        {
            if (Input.GetKeyDown(listKey[i]))
            {
                isLongPress = true;
            }

            if (Input.GetKeyUp(listKey[i]))
            {
                isLongPress = false;
            }
        }
    }
}
