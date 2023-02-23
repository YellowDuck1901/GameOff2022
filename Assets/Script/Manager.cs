using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static Manager Instance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = gameObject.GetComponent<Manager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
