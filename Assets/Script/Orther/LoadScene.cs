using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
     private string NextSence;

    [SerializeField]
    private float ColdDownTime;

    private void Start()
    {
    }
    public IEnumerator WithColdDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(ColdDownTime);
            SceneManager.LoadScene(NextSence);
            break;
        }
    }

    public void openSceneWithColdDown()
    {
        StartCoroutine(WithColdDown());
    }

   

}
