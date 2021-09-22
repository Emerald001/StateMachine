using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    public int Health { get => health; protected set => health = value; }

    private StateMachine<Enemy> coupledStateMachine;

    void Start()
    {
        Debug.Log("Started to create");

        coupledStateMachine = new StateMachine<Enemy>(this);
        IdleEnemyState idleState = new IdleEnemyState(coupledStateMachine);
        AttackEnemyState attackState = new AttackEnemyState(coupledStateMachine);

        idleState.AddTransition(new Transition<Enemy>(
            (x) =>
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    return true;
                return false;
            }, typeof(AttackEnemyState)));

        attackState.AddTransition(new Transition<Enemy>(
            (x) =>
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                    return true;
                return false;
            }, typeof(IdleEnemyState)));

        coupledStateMachine.AddState(typeof(IdleEnemyState), idleState);
        coupledStateMachine.AddState(typeof(AttackEnemyState), attackState);

        coupledStateMachine.SwitchState(typeof(IdleEnemyState));
    }

    private void Update()
    {
        coupledStateMachine.RunUpdate();
    }
}

public class StateMachine<T>
{
    private State<T> currentState;

    private Dictionary<System.Type, State<T>> stateDictionary = new Dictionary<System.Type, State<T>>();

    public T Controller { get; protected set; }

    public StateMachine(T _owner)
    {
        Debug.Log("StateMachine Made");
        Controller = _owner;
    }

    public void SwitchState(System.Type _switcher)
    {
        if (currentState != null)
        {
            //currentState.OnExit();
        }

        if (stateDictionary.ContainsKey(_switcher))
        {
            Debug.Log("Switching state");
            var tmpState = stateDictionary[_switcher];

            Debug.Log(tmpState);

            tmpState.OnEnter();
            currentState = tmpState;
            return;
        }
        Debug.Log("Type not in Dictionairy");
    }

    public void AddState(System.Type _type, State<T> _state)
    {
        if (stateDictionary.ContainsValue(_state))
        {
            Debug.LogError("Already added State");
            return;
        }
        stateDictionary.Add(_type, _state);
    }

    public void RemoveState(System.Type _type)
    {
        if (!stateDictionary.ContainsKey(_type))
        {
            Debug.LogError("State not in the list");
            return;
        }
        stateDictionary.Remove(_type);
    }

    public void RunUpdate()
    {
        currentState.OnUpdate();
    }
}

public abstract class State<T>
{
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
    public StateMachine<T> stateMachine { get; protected set; }
    public List<Transition<T>> transitions = new List<Transition<T>>();

    public State(StateMachine<T> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void AddTransition(Transition<T> transition)
    {
        Debug.Log("Added transition");

        transitions.Add(transition);
    }
}

public class Transition<T>
{
    public Transition(System.Predicate<T> condition, System.Type toState)
    {
        this.condition = condition;
        this.toState = toState;
    }
    public System.Predicate<T> condition;
    public System.Type toState;
}

public class EnemyState : State<Enemy>
{
    public EnemyState(StateMachine<Enemy> stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdate()
    {
        foreach (Transition<Enemy> transition in transitions)
        {
            if (transition.condition.Invoke(stateMachine.Controller))
            {
                stateMachine.SwitchState(transition.toState);
                return;
            }
        }
    }
}

public class IdleEnemyState : EnemyState
{ 
    public IdleEnemyState(StateMachine<Enemy> _owner) : base(_owner)
    {
    }

    public override void OnEnter()
    {
        Debug.Log("First State");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }
}

public class AttackEnemyState : EnemyState
{
    public AttackEnemyState(StateMachine<Enemy> _owner) : base(_owner)
    {

    }

    public override void OnEnter()
    {
        Debug.Log("Second State");
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

    }
}
