using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    private BoxCollider2D playerCollider;
    [SerializeField] private float timeIgnore = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine(DisableCollision()); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
    
    private IEnumerator DisableCollision()
    {
        BoxCollider2D PlatformCollider2D = gameObject.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, PlatformCollider2D);
        yield return new WaitForSeconds(timeIgnore);
        Physics2D.IgnoreCollision(playerCollider, PlatformCollider2D,false);

    }
}
