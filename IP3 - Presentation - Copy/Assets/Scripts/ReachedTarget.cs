using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachedTarget : CharacterBehaviour
{
    //private int pressCounter;
    [SerializeField] private byte buildingPressReq;
    [SerializeField] private byte playerPressReq;
    [SerializeField] private byte targetBreakPressReq;

    private int pickUpPressCount;
    private int breakPressCount;

    private GameObject target;
    private MoveToSide mts;
    private Controls controls;

    [SerializeField] float animDelayTimer;
    bool animDelay;

    public void Start()
    {
        if (GetComponent<MoveToSide>())
            mts = GetComponent<MoveToSide>();
        else
            Debug.Log("Component Move To Side is missing");

        animDelay = false;
        controls = GetComponent<Player>().GetControls();
    }

    private IEnumerator DelayState()
    {
        animDelay = true;
        yield return new WaitForSeconds(animDelayTimer);
        animDelay = false;
    }

    public override bool Condition(Player.Actions state)
    {
        target = mts.GetTarget();

        if (state == Player.Actions.HOLDBUILDING)
            return false;

        if (target != null && target.tag == "Player")
            if (!target.GetComponent<MoveToSide>().GetTargetReached())           
                return false;
        if (mts.GetTargetReached() && target != null)
            return true;

        return false;
    }

    public override void ExcecuteBehaviour()
    {
        if (mts.GetTarget().tag == "Building")
        {
            PickUpButtonPressed();
            if(mts.GetComponent<BoxCollider>())
                mts.GetComponent<BoxCollider>().enabled = false;
        }

        if (target.tag == "Player" &&
            target.GetComponent<MoveToSide>().GetTargetReached())
        {
            if (target.GetComponent<Player>().GetState() == Player.Actions.HOLDBUILDING)
            {
                BreakButtonPressed();
            }
            else if (!target.GetComponent<ReachedTarget>().PickUpButtonsPressed())
            {
                PickUpButtonPressed();
            }
        }
    }

    private void BreakButtonPressed()
    {
        if (Input.GetKeyDown(controls.GetPickKey()) || Input.GetKeyDown(controls.GetPicKJoyStick()))
            breakPressCount++;

        if (breakPressCount >= targetBreakPressReq)
        {
            Break(gameObject);
            Break(target);
            breakPressCount = 0;
        }
    }

    private void Break(GameObject objToClear)
    {
        objToClear.GetComponent<MoveToSide>().ResetData();
        objToClear.GetComponent<ReachedTarget>().ResetPresses();
    }

    private void PickUpButtonPressed()
    {
        if (Input.GetKeyDown(controls.GetPickKey()) || Input.GetKeyDown(controls.GetPicKJoyStick()))
        {
            pickUpPressCount++;
        }
    }

    public void ResetPresses() { pickUpPressCount = 0; }

    public bool PickUpButtonsPressed()
    {
        if (target == null)
            return false;

        byte currentPressesReq = 0;

        if (target.tag == "Player")        
            currentPressesReq = playerPressReq;        
        else if (target.tag == "Building")
            currentPressesReq = buildingPressReq;
        if (currentPressesReq == 0)
            return false;

        if (pickUpPressCount >= currentPressesReq)
        {
            StartCoroutine(DelayState());
            return true;
        }
        else
            return false;
    }
}
