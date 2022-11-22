using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PreventNextLevel : MonoBehaviour
{
    public static bool isNextLevel = false;

    public static bool isPreventNextLevelDialogue = false;

    [SerializeField] List<Collect> EventPrevent = new List<Collect>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            EventPrevent = EventPrevent.Where(a => a.isCollision == false).ToList();
            if (EventPrevent.Count == 0)
            {
                isNextLevel = true;
            }
            else
            {
                isPreventNextLevelDialogue = true;
            }
        }
    }


}
