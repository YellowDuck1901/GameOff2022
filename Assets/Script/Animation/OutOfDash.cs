using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class OutOfDash : MonoBehaviour
{
    public GameObject playerSprite;
    public GameObject outOfDashSprite;

    public List<Sprite> outOfDashSprites = new List<Sprite>();


    private void Update()
    {
        String currentSpriteString = playerSprite.GetComponent<SpriteRenderer>().sprite.name;

        string resultString = Regex.Match(currentSpriteString, @"\d+").Value;

        int yes = Int32.Parse(resultString);

        outOfDashSprite.GetComponent<SpriteRenderer>().sprite = outOfDashSprites[yes];
    }
}
