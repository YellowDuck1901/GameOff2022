using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{

	[SerializeField] TextAsset inkJSON;
	[SerializeField] UnControllerData[] UnControllerDatas;

    public bool isTrigger;
    Mechanic mechanic = new Mechanic();
    // Update is called once per frame
    void Update()
    {
        if (isTrigger)
        {
            isTrigger = !isTrigger;
            DialogueManager.getInstance().EnterDialogueMode(inkJSON);

            foreach(var controller in UnControllerDatas)
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
    }
}

