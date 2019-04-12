using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : CharacterBehaviour
{
    [SerializeField] private float force;
    [SerializeField] public float verticalPush;

    private GameObject target;

    private bool throwing;
    [SerializeField] private float delayThrow;
    [SerializeField] private float delayChangeState;

    private void Start()
    {
        throwing = false;
    }

    public override bool Condition(Player.Actions state)
    {
        if (throwing)
            return true;

        Controls controls = GetComponent<Player>().GetControls();
        if ((Input.GetKeyDown(controls.GetPickKey()) || Input.GetKeyDown(controls.GetPicKJoyStick())) &&
            state == Player.Actions.HOLDBUILDING &&
            GetComponent<ReachedTarget>().PickUpButtonsPressed())     
            return true;     
        return false;
    }

    private IEnumerator DelayToThrow()
    {
        if (GetComponent<MoveToSide>().GetTargetTrans().gameObject != null)
        {
            target = GetComponent<MoveToSide>().GetTargetTrans().gameObject;
            target.GetComponent<Collider>().enabled = true;
        }
        throwing = true;

        yield return new WaitForSeconds(delayThrow);
        ThrowBuilding();
        GetComponent<MoveToSide>().ResetData();
        GetComponent<ReachedTarget>().ResetPresses();
        GetComponent<CharacterController>().enabled = true;
        StartCoroutine(DelayChangeState());
    }

    private IEnumerator DelayChangeState()
    {
        yield return new WaitForSeconds(delayChangeState);
        throwing = false;
    }

    public override void ExcecuteBehaviour()
    {
        if(!throwing)
            StartCoroutine(DelayToThrow());
    }

    private void ThrowBuilding()
    {
        Rigidbody buildingRig = target.GetComponent<Rigidbody>();
        Vector3 buildingPos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 mosnterPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 verticalPushVector = new Vector3(0, verticalPush, 0);

        if(target.GetComponent<Thrown>())
            target.GetComponent<Thrown>().SetThrown(true);

         buildingRig.isKinematic = false;
         buildingRig.AddForce((buildingPos - mosnterPos + verticalPushVector).normalized * force);

        if(target.GetComponent<BuildingStats>())
            target.GetComponent<BuildingStats>().thrown = true;
    }
}
