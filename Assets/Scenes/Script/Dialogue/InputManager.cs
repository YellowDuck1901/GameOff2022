using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private bool interactPressed = false;



    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Input Manager in the scene.");
        }else instance = this;
    }

    public static InputManager GetInstance()
    {
        return instance;
    }


    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactPressed = true;
        }
        else if (context.canceled)
        {
            interactPressed = false;
        }
    }


    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetInteractPressed()
    {
        bool result = interactPressed;
        interactPressed = false;
        return result;
    }


}
