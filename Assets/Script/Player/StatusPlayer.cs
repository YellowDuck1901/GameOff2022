using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPlayer : MonoBehaviour
{
    [SerializeField]
    private bool isDead, isImmortal, isCutSence;

    private static StatusPlayer playerInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void OnLevelWasLoaded(int level)
    {
        FindStartPos();
    }

    void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPosition").transform.position;
    }
}
