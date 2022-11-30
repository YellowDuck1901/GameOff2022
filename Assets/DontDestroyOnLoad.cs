using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad DontDestroyInstance;
    private void Awake()
    {
        if (DontDestroyInstance != null)
        {
            Destroy(gameObject);
        }else
            DontDestroyInstance = this;

        DontDestroyOnLoad(gameObject);
    }
}
