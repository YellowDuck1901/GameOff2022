using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "UnController Data")]
public class UnControllerData : ScriptableObject
{
    public enum FunctionOption
    {
        limitNumberMovement,
        limitTimePressMovement,
        doActionInTimeRange,
        setDisableMovement
    };

    [Header("Function Un-Controller")]
    [Space(5)]
    [SerializeField]
    public FunctionOption _selectedFunction;

    [SerializeField]
    public Mechanic.Movements Movement;

    [SerializeField]
    public int NumberMovement;

    [SerializeField]
    public float LimitTime;

    [SerializeField]
    public bool isDisable;

    [SerializeField]
    public bool triggerFunction;

    private Mechanic mechanic = new Mechanic();
    //private void OnValidate()
    //{
    //    if (triggerFunction)
    //    {
    //        switch (_selectedFunction)
    //        {
    //            case UnControllerData.FunctionOption.limitNumberMovement:
    //                StartCoroutine(mechanic.limitNumberMovement(Movement,NumberMovement));
    //                break;
    //            case UnControllerData.FunctionOption.limitTimePressMovement:
    //                StartCoroutine(mechanic.limitTimePressMovement(Movement, LimitTime));
    //                break;
    //            case UnControllerData.FunctionOption.doActionInTimeRange:
    //                StartCoroutine(mechanic.doActionInTimeRange(Movement, NumberMovement, LimitTime));
    //                break;
    //            case UnControllerData.FunctionOption.setDisableMovement:
    //                mechanic.setDisableMovement(Movement,isDisable);
    //                break;
    //        }
    //    }
    //}
}
