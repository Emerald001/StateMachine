using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSMTest;

public class Player : MonoBehaviour, IStateRunner
{
    public ScratchPad sharedData { get; set; }
    public StateMachine statemachine;

    private void Start()
    {
        statemachine = new StateMachine(this);
        sharedData = new ScratchPad();

        sharedData.Set("Grounded", new GroundedState());
        sharedData.Set("Airborn", new AirbornState());

        statemachine.SetState(sharedData.Get<IState>("Grounded"));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            statemachine.SetState(sharedData.Get<IState>("Airborn"));
        }
    }
}
