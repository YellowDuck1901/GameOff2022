using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasPause : MonoBehaviour
{
    public GameObject PannelPause;

    private static CanvasPause canvasPause;

    public string[] skipScene;

    private string currentScene;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (canvasPause == null)
        {
            canvasPause = gameObject.GetComponent<CanvasPause>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        currentScene = SceneManager.GetActiveScene().name;
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(currentScene);
            if (!skipScene.Any(s => currentScene.Trim().Equals(s.Trim())))
            {
                if (PannelPause.active)
                {
                    Unpause();
                    PannelPause.SetActive(false);
                }
                else
                {
                    Pause();
                    PannelPause.SetActive(true);
                }
            }
        }
    }

    public void Pause()
    {
        Manager_SBG.stopPlay();
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Manager_SBG.PlaySound(soundsGame.BackGround);

        Time.timeScale = 1;
    }

}
