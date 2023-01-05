using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool  IsPentlyThisLevel;
    [SerializeField] UnControllerData[] UnControllerFunctionPenalty;

    [SerializeField] LoadScene loadScene;

    private bool triggerDefault;
    public static bool triggerPenalty;

    Mechanic mechanic = new Mechanic();

    [SerializeField]
    public class EventDialogue
    {
        
    }

    private void Start()
    {
        if (IsPentlyThisLevel && PenatlyManager.Penatly)
        {
            DialogueManager.getInstance().EnterDialogueMode(inkJSON, KnotLevel + strKnotPenatly);
        }
        else
        {
            DialogueManager.getInstance().EnterDialogueMode(inkJSON, KnotLevel);

        }

        if (!SceneManager.GetActiveScene().name.Equals("Menu"))
        {
            if (SceneManager.GetActiveScene().name.Equals("Ending"))
            {
                Manager_SBG.PlaySound(soundsGame.EndingMusic);
            }
            else
            {
                Manager_SBG.PlaySound(soundsGame.BackGround);
            }
        }

    }

    private void Update()
    {

        if (StatusPlayer.playerInstance != null && StatusPlayer.playerInstance.IsDead)
        {
            triggerDefault = false;
        }

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
                    case UnControllerData.FunctionOption.limitAndDisableNumberMovement:
                        StartCoroutine(mechanic.limitAndDisableNumberMovement(controller.Movement, controller.NumberMovement));
                        break;
                }
            }
        }
        if (IsPentlyThisLevel  &&  !triggerPenalty && PenatlyManager.Penatly && !DialogueManager.dialogueIsPlaying) //this level have penatly
        {
            triggerPenalty = true;

            //dialogue penatly
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
                    case UnControllerData.FunctionOption.limitAndDisableNumberMovement:
                        StartCoroutine(mechanic.limitAndDisableNumberMovement(controller.Movement, controller.NumberMovement)) ;
                        break;
                }
            }
        }

        //next level
        if (PreventNextLevel.isNextLevel)
        {
            PreventNextLevel.isNextLevel = false;
            loadScene.openSceneWithColdDown();
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

