using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
     public string NextScene;

    [SerializeField]
    public float ColdDownTime;

    private void Start()
    {
    }
    public IEnumerator WithColdDown(string NextScene, float ColdDownTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(ColdDownTime);
            SceneManager.LoadScene(NextScene);
            break;
        }
    }

    public void openSceneWithColdDown(string NextScene, float ColdDownTime)
    {
        Debug.Log(NextScene);
        StartCoroutine(WithColdDown(NextScene, ColdDownTime));
    }

   

}
