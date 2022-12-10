using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayActive : MonoBehaviour
{
    [Serializable]
    private class EntityDelay
    {
        public float delayTime;
        public GameObject gameObject;
        public bool isActive;
    }

    [SerializeField]
    private EntityDelay[] entityDelays;

        
    // Start is called before the first frame update
    void Start()
    {
        if(entityDelays.Length  > 0 )
        {
            foreach (var entity in entityDelays)
            {
                entity.gameObject.SetActive(false);
                StartCoroutine(delayActiveGameObject(entity.delayTime, entity.gameObject, entity.isActive));
            }
        }
    }

    IEnumerator delayActiveGameObject(float delayTime, GameObject gameObject,bool isActive)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.SetActive(isActive);
    }
}
