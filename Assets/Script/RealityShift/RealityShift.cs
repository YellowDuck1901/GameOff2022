using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityShift : MonoBehaviour
{
    public GameObject map1;
    public GameObject map2;
    public bool map1Active;

    void Start()
    {
        map1.SetActive(true);
        map2.SetActive(false);
        map1Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(map1Active)
            {
                map1.SetActive(false);
                map2.SetActive(true);
                map1Active = false;
            }
            else
            {
                map1.SetActive(true);
                map2.SetActive(false);
                map1Active = true;
            }
        }
    }
}
