using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    [Header("Script Game")]
    [SerializeField] string KnotLevel;
    private string strKnotPenatly = "_Penalty";
    private string strKnotCollect = "_Collect";
    private string strKnotPreventNextLevel = "_Prevent";



    [SerializeField] TextAsset inkJSON;

    [Space(10)]
    [Header("UnController In Level")]
    [SerializeField] UnControllerData[] UnControllerFunctionDefault;

    [Space(10)]
    [Header("UnController Penalty")]
    [SerializeField] bool  IsPentlyThisLevel;
    [SerializeField] UnControllerData[] UnControllerFunctionPenalty;

    [SerializeField] LoadScene loadScene;

    private bool triggerDefault;
    private bool triggerPenalty;

    Mechanic mechanic = new Mechanic();

    [SerializeField]
    public class EventDialogue
    {
        
    }

    private void Start()
    {
        if (IsPentlyThisLevel)
        {
            DialogueManager.getInstance().EnterDialogueMode(inkJSON, KnotLevel + strKnotPenatly);
        }
            DialogueManager.getInstance().EnterDialogueMode(inkJSON, KnotLevel);

    }

    private void Update()
    {
        if (!triggerDefault && !DialogueManager.dialogueIsPlaying )
        {
            triggerDefault = true;
            foreach (var controller in UnControllerFunctionDefault)
            {
                switch (controller._selectedFunction)
                {
                    case UnControllerData.FunctionOption.limitNumberMovement:
                        StartCoroutine(mechanic.limitNumberMovement(controller.Movement, controller.NumberMovement));
                        break;
                    case UnControllerData.FunctionOption.limitTimePressMovement:
                        StartCoroutine(mechanic.limitTimePressMovement(controller.Movement, controller.LimitTime));
                        break;
                    case UnControllerData.FunctionOption.doActionInTimeRange:
                        StartCoroutine(mechanic.doActionInTimeRange(controller.Movement, controller.NumberMovement, controller.LimitTime));
                        break;
                    case UnControllerData.FunctionOption.setDisableMovement:
                        mechanic.setDisableMovement(controller.Movement, controller.isDisable);
                        break;
                }
            }
        }

        if (IsPentlyThisLevel  &&  !triggerPenalty && PenatlyManager.Penatly && !DialogueManager.dialogueIsPlaying) //this level have penatly
        {
            triggerPenalty = true;
            PenatlyManager.Penatly = false;

            //dialogue penatly
            Debug.Log("trigger Penalty this level");
        
            foreach (var controller in UnControllerFunctionPenalty)
            {
                switch (controller._selectedFunction)
                {
                    case UnControllerData.FunctionOption.limitNumberMovement:
                        StartCoroutine(mechanic.limitNumberMovement(controller.Movement, controller.NumberMovement));
                        break;
                    case UnControllerData.FunctionOption.limitTimePressMovement:
                        StartCoroutine(mechanic.limitTimePressMovement(controller.Movement, controller.LimitTime));
                        break;
                    case UnControllerData.FunctionOption.doActionInTimeRange:
                        StartCoroutine(mechanic.doActionInTimeRange(controller.Movement, controller.NumberMovement, controller.LimitTime));
                        break;
                    case UnControllerData.FunctionOption.setDisableMovement:
                        mechanic.setDisableMovement(controller.Movement, controller.isDisable);
                        break;
                }
            }
        }

        //next level
        if (PreventNextLevel.isNextLevel)
        {
            PreventNextLevel.isNextLevel = false;
            loadScene.openSceneWithColdDown(loadScene.NextScene, loadScene.ColdDownTime);
        }

        //prevent next level
        else if(PreventNextLevel.isPreventNextLevelDialogue) 
        {
            PreventNextLevel.isPreventNextLevelDialogue = false;
            DialogueManager.getInstance().EnterDialogueMode(inkJSON,KnotLevel + strKnotPreventNextLevel);
        }

        //dialogue when ever trigger collect event
        if (Collect.isCollectDialogue)
        {
            Collect.isCollectDialogue = false;
            DialogueManager.getInstance().EnterDialogueMode(inkJSON, KnotLevel + strKnotCollect);
        }
    }

    IEnumerator delay(float time)
    {
        yield return new WaitForSeconds(time);
    }
}

