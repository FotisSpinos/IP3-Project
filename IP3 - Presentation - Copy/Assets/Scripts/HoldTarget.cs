using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TargetFunctions), typeof(PosFunctions))]
public class HoldTarget : CharacterBehaviour
{
    // Building store variables
    private TargetFunctions bf;
    private PosFunctions pf;
    private Player player;

    private GameObject target;

    private void Start()
    {
        pf = GetComponent<PosFunctions>();
        bf = GetComponent<TargetFunctions>();
        player = GetComponent<Player>();

        bf.Initialize();
    }

    public override bool Condition(Player.Actions state)
    {
        ReachedTarget rt = GetComponent<ReachedTarget>();

        if (rt.PickUpButtonsPressed())        
            return ErrorCheckBehaviour();        
        
        return false;
    }

    private bool ErrorCheckBehaviour()
    {
        target = GetComponent<MoveToSide>().GetTargetTrans().gameObject;

        if (target == null)
            return false;

        // if target is not a character and target is not a building then dont excecute (Target invalid)
        if (target.tag == "Player" && target.tag == "Building")
            return false;

        if (target.GetComponent<MeshRenderer>() == null)
            return false;
        return true;
    }

    public override void ExcecuteBehaviour()    //Define buildings and players thats the ERROR
    {
        // This could run once
        if (target.GetComponent<BuildingStats>())
            target.GetComponent<BuildingStats>().SetPlayerPick(gameObject);   //instant missing on pick up 

        // This could be running once
        if(target.GetComponent<BoxCollider>())
            target.GetComponent<BoxCollider>().enabled = false;

        // Actual pick up behaviour
        Vector3 characterScale = GetComponent<CharacterController>().bounds.size;
        Vector3 targetScale = target.GetComponent<MeshRenderer>().bounds.size;
        Vector3 facingDirection = transform.forward.normalized;

        // Move
        target.transform.position = transform.position + new Vector3(facingDirection.x * 2f, facingDirection.y * characterScale.y * targetScale.y / 2, facingDirection.z * 2f) + new Vector3(0, 0.5f, 0.0f);

        // Rotate
        Vector3 targetDir = (target.transform.localPosition - transform.localPosition).normalized;
        //Vector3 newDir = Vector3.RotateTowards((transform.forward), targetDir, 1, 0.0f);
        Quaternion newDir = Quaternion.LookRotation(new Vector3(targetDir.x,0,targetDir.z));
        target.transform.rotation = newDir;

        //Excecute walk behaviour 
        GetComponent<Walk>().ExcecuteBehaviour();
    }
}