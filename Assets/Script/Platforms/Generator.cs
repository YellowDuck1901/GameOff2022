using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject;
    [SerializeField] float coldDown;
    [SerializeField] bool IsSetActive;

    private bool isDelay;
    IEnumerator GeneratorObject(GameObject gameObject, float coldDown)
    {
        isDelay = true;
        yield return new WaitForSeconds(coldDown);
        Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
        isDelay = false;
    }

    IEnumerator SetActiveObject(GameObject gameObject, float coldDown, bool IsActive)
    {
        isDelay = true;
        yield return new WaitForSeconds(coldDown);
        gameObject.SetActive(true);
        isDelay = false;

    }

    private void Update()
    {
        if (!isDelay && !gameObject.active || gameObject==null)
        {
            if (IsSetActive)
            {
                StartCoroutine(SetActiveObject(gameObject, coldDown, true)); 
            }

        }

        
    }

  

}
