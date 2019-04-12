using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrestle : CharacterBehaviour
{
    private MoveToSide mts;
    private Controls controls;

    public void Start()
    {
        if (GetComponent<MoveToSide>())
            mts = GetComponent<MoveToSide>();
        else
            Debug.Log("Component Move To Side is missing");

        controls = GetComponent<Player>().GetControls();
    }

    public override bool Condition(Player.Actions state)
    {
        if (mts.GetTarget() && mts.GetTarget().tag == "Player")
            if (mts.GetTarget().GetComponent<MoveToSide>().GetTargetReached())            
                return true;
            
        return false;
    }

    public override void ExcecuteBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
