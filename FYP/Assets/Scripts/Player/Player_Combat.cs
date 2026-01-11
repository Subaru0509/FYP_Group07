using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = .1f;
    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;

        foreach ( var target in GetDetectedColiders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();

            if (counterable != null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasPerformedCounter = true;
            }
        }
        return hasPerformedCounter;
    }
    
    public float GetCounterRecoveryDuration() => counterRecovery;
}
