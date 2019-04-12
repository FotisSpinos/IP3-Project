using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown : CharacterBehaviour
{
    [SerializeField] private float thrownTimer;
    private bool thrown;
    private bool corutineStarted;

    public void Start()
    {
        thrown = false;
        corutineStarted = false;
    }
    public void SetThrown(bool thrown) { this.thrown = thrown; }

    public override bool Condition(Player.Actions state)
    {
        if (thrown) 
            return true;
        
        return false;
    }

    public override void ExcecuteBehaviour()
    {
        if (!corutineStarted)
            StartCoroutine(ThrownBehaviour());
    }

    IEnumerator ThrownBehaviour()
    {
        GetComponent<ReachedTarget>().ResetPresses();
        GetComponent<MoveToSide>().ResetData();
        GetComponent<CapsuleCollider>().enabled = true;
        corutineStarted = true;

        yield return new WaitForSeconds(thrownTimer);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        corutineStarted = false;
        thrown = false;
    }
}