using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isTrigger;

    [SerializeField]
    private TextAsset inkJSON;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrigger)
        {
            isTrigger = !isTrigger;
            DialogueManager.getInstance().EnterDialogueMode(inkJSON);
        }
    }


    
}
