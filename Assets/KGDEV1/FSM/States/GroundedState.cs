using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMTest;

public class GroundedState : AState
{
    public override void Start(IStateRunner owner) 
    {
        Debug.Log("Grounded State");
    }
}