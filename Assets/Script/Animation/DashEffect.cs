using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySecond;
    public GameObject ghost;
    public bool makeGhost = false;

    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySecond = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {

            if (ghostDelaySecond > 0)
            {
                ghostDelaySecond -= Time.deltaTime;
            }
            else
            {
                GameObject curretGhost = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                curretGhost.transform.localScale = this.transform.localScale;
                curretGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;  
                ghostDelaySecond = ghostDelay;
                Destroy(curretGhost,1f);
            }
        }
    }
}
