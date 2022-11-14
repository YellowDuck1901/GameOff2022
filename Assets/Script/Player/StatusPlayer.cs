using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPlayer : MonoBehaviour
{
    [SerializeField]
    private bool isDead, isImmortal, isCutSence;

    public bool IsDead
    {
        set
        {
            if (!IsImmortal)
                this.isDead = value;
        }
        get { return this.isDead; }
    }

    public bool IsImmortal
    {
        set
        {
            this.isImmortal = value;
        }
        get
        {
            return this.isImmortal;
        }
    }

    public bool IsCutSence
    {   
        set
        {
            this.isCutSence = value;
        }
        get
        {
            return this.isCutSence;
        }
    }
       

}
