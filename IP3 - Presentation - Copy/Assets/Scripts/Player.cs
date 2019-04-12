using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Controls))]
[RequireComponent(typeof(HoldTarget))]
[RequireComponent(typeof(MoveToSide))]
[RequireComponent(typeof(Throw))]
[RequireComponent(typeof(Walk))]
[RequireComponent(typeof(Dash))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(ReachedTarget))]
[RequireComponent(typeof(Thrown))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class Player : MonoBehaviour
{ 

    public enum Actions
    {
        ATTACK,
        HOLDBUILDING,
        MOVETOSIDE,
        REACHEDTOTARGET,
        THROW,
        WALK,
        DASH,
        IDLE,
        THROWN,
        WRESTLE
    }
    private Actions state;

    Actions[] lockActions;

    [SerializeField] private int playerIndex;
    private Controls controls;

    private HoldTarget pickObject;
    private MoveToSide moveToPoint;
    private Throw throwBuilding;
    private Walk walk;
    private Dash dash;
    private Attack attack;
    private ReachedTarget reachedTarget;
    private Thrown thrown;
    private Idle idle;
    private CharacterBehaviour currentBehaviour;

    private Actions currentState;
    private Actions lastState;

    public delegate void PlaySound(int playerNum, Player.Actions playerState, Transform trans);
    public static event PlaySound OnPlaySound;

    private void OnEnable()
    {
        lockActions = new Actions[5];

        // Generate Controlls
        controls = GetComponent<Controls>();  //add component 
        controls.SetControlls(playerIndex);

        state = Actions.IDLE;

        pickObject = GetComponent<HoldTarget>();
        moveToPoint = GetComponent<MoveToSide>();
        throwBuilding = GetComponent<Throw>();
        walk = GetComponent<Walk>();
        attack = GetComponent<Attack>();
        dash = GetComponent<Dash>();
        reachedTarget = GetComponent<ReachedTarget>();
        thrown = GetComponent<Thrown>();
        idle = GetComponent<Idle>();

        currentBehaviour = walk;
    }

    private Actions DefineState()
    {
        if (idle.Condition(lastState))
            state = Actions.IDLE;
        else if(attack.Condition(lastState))
            state = Actions.ATTACK;
        else if (thrown.Condition(lastState))
            state = Actions.THROWN;
        else if (throwBuilding.Condition(lastState))
            state = Actions.THROW;
        else if (pickObject.Condition(lastState))
            state = Actions.HOLDBUILDING;
        else if (reachedTarget.Condition(lastState))
            state = Actions.REACHEDTOTARGET;
        else if (moveToPoint.Condition(lastState))
            state = Actions.MOVETOSIDE;
        else if (dash.Condition(lastState))
            state = Actions.DASH;
        else if (walk.Condition(lastState))
            state = Actions.WALK;
        else
            state = Actions.IDLE;

        return state;
    }

    private void Update()
    {
        lastState = state;
        currentState = DefineState();
        
        switch (currentState)
        {
            case Actions.ATTACK:
                currentBehaviour = attack;             
                break;

            case Actions.WALK:
                currentBehaviour = walk;
                break;

            case Actions.DASH:
                currentBehaviour = dash;
                break;

            case Actions.MOVETOSIDE:
                currentBehaviour = moveToPoint;
                break;

            case Actions.REACHEDTOTARGET:
                currentBehaviour = reachedTarget;
                break;

            case Actions.HOLDBUILDING:
                currentBehaviour = pickObject;
                break;

            case Actions.THROW:
                currentBehaviour = throwBuilding;
                break;

            case Actions.THROWN:
                currentBehaviour = thrown;
                break;

            case Actions.IDLE:
                currentBehaviour = null;
                break;
        }

        if(currentBehaviour != null)
            currentBehaviour.ExcecuteBehaviour();

        GiveFeedback();
    }

    [SerializeField] AnimationManager AnimCont;
    [SerializeField] GameObject dashObject;

    // Gives visual and audio feedback
    public void GiveFeedback()
    {
        if(lastState != currentState)
        {
            OnPlaySound(playerIndex, currentState, transform);
            AnimCont.UpdateAnimation(currentState);

            if (currentState == Actions.DASH && dashObject != null)
                dashObject.SetActive(true);
        }

        if (lastState == Actions.DASH && currentState != Actions.DASH)
            StartCoroutine(WaitToDeactiveteParticle());
    }

    private IEnumerator WaitToDeactiveteParticle()
    {
        yield return new WaitForSeconds(1);
        dashObject.SetActive(false);
    }

    public Actions GetState() { return state; }
    public Actions GetLastState() { return lastState; }
    public Controls GetControls() { return controls; }
    public int GetPlayerIndex() { return playerIndex; }
}
 