using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusPlayer : MonoBehaviour
{
    [SerializeField]
    private bool isDead, isImmortal, isCutSence;

    private static StatusPlayer playerInstance;

    private LoadScene LoadScene;

    private void Start()
    {
        LoadScene = GameObject.Find("LoadLevel").GetComponent<LoadScene>();
    }
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
            if (this.isDead)
            {
                this.isDead = false;
                Debug.Log("Load Scene");
                if(LoadScene == null)
                {
                    LoadScene.openSceneWithColdDown(SceneManager.GetActiveScene().name, 0f);
                }
                FindStartPos();
            }
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
        LoadScene = GameObject.Find("LoadLevel").GetComponent<LoadScene>();
        FindStartPos();
    }

    public void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPosition").transform.position;
    }
}
