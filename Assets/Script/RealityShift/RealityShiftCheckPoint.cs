using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityShiftCheckPoint : MonoBehaviour
{
    public GameObject map1;
    public GameObject map2;
    public GameObject bg1;
    public GameObject bg2;
    public bool map1Active;


    private void Start()
    {
        map1 = GameObject.Find("Map1");
        map2 = GameObject.Find("Map2");
    }

    private void Update()
    {
        if (map1.activeSelf)
        {
            map1Active = true;
            bg1.SetActive(true);
            bg2.SetActive(false);
        }
        else
        {
            map1Active = false;
            bg1.SetActive(false);
            bg2.SetActive(true);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if (map1Active)
            {
                map1.SetActive(false);
                map2.SetActive(true);
                bg1.SetActive(false);
                bg2.SetActive(true);
                map1Active = false;
            }
            else
            {
                map1.SetActive(true);
                map2.SetActive(false);
                bg1.SetActive(true);
                bg2.SetActive(false);
                map1Active = true;
            }
            Destroy(gameObject);
        }
    }
}
