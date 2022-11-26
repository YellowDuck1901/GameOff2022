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
        setDisableMovement,
        limitAndDisableNumberMovement
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
}
