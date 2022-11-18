using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignLevel : MonoBehaviour
{

    [Header("Script In Level")]
    [SerializeField] private TextAsset[] inkJSON;

    Mechanic mechanic = new Mechanic();
    
    void setMechanicUnController(string Level)
    {
        switch (Level)
        {
            case "1":
                List<System.Action> actions = new List<System.Action>();
                break;
        }
    }
    

}
