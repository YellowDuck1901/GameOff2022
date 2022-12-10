using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChildrenObject
{
    public static GameObject GetChildWithName(GameObject Parrent, string name)
    {
        Transform trans = Parrent.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}
