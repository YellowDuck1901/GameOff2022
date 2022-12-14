using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.PackageManager;
using UnityEditor.TextCore.Text;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusPlayer : MonoBehaviour
{
    [SerializeField]
    private bool isDead, isImmortal, isCutSence, isHit, isPlayingDeadAnim; 

    public static StatusPlayer playerInstance;

    private LoadScene LoadScene;

    public PlayerSound _PlayerSound;

    private PlayerMovement playerMovement;

    public Animator anim;


    private void Start()
    {
        LoadScene = GameObject.Find("LoadLevel").GetComponent<LoadScene>();
        playerMovement = gameObject.GetComponent<PlayerMovement>(); 
    }
    void Awake()
    {

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

    public bool IsHit
    {
        set
        {
            this.isHit = value;
        }
        get
        {
            return this.isHit;
        }
    }

    public void OnLevelWasLoaded(int level)
    {
        Mechanic.resetDisableMovement();
        LoadScene = GameObject.Find("LoadLevel").GetComponent<LoadScene>();
        FindStartPos();
    }

    public void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPosition").transform.position;
    }

    private void LateUpdate()
    {
        

        if (isDead && !isPlayingDeadAnim)
        {
            Debug.Log("Dead");
            isDead = false;
            isPlayingDeadAnim = true;
            //if (LoadScene == null)
            //{
            //    LoadScene.openSceneWithColdDown();
            //}
            LevelData.triggerPenalty = false;
            StartCoroutine(DeadAnimation(1f));
            
        }

        if (IsHit && !isPlayingDeadAnim)
        {
            Mechanic.resetDisableMovement();
            PlayerMovement._disableAllMovement = true;
            IsHit = false;
            isDead = true;
        }


    }

    IEnumerator DeadAnimation(float delayTime)
    {
        playerMovement._isDead = true;
        playerMovement.RB.constraints = RigidbodyConstraints2D.FreezeAll;


        yield return new WaitForSeconds(0.2f);

        anim.SetTrigger("Start");
        yield return new WaitForSeconds(0.6f);
        anim.SetTrigger("End");
        yield return new WaitForSeconds(0.2f);
        FindStartPos();
        playerMovement._isDead = false;
        playerMovement._isPlayingDeadAnim = false;

        isPlayingDeadAnim = false;
        PlayerMovement._disableAllMovement = false;

        playerMovement.RB.constraints = RigidbodyConstraints2D.None;
        playerMovement.RB.constraints = RigidbodyConstraints2D.FreezeRotation;


    }




}
