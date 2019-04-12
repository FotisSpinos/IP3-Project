using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToSide : CharacterBehaviour
{
    // Monster Characteristics
    [SerializeField] private float pickUpRange;
    [SerializeField] private float distPerSec;
    [SerializeField] private float rotPerSec;

    [SerializeField] private float validationTimer;
    [SerializeField] private float maxDistError;
    [SerializeField] private float maxAngleError;

    private GameObject targetGameObject;
    private Vector3 targetPoint;
    private Transform targetTrasform;
    private PosFunctions posF;
    private bool targetReached;
    private bool movingToPoint;
    private Vector3 rotationPoint;
    private bool attacked;

    public void OnEnable()
    {
        posF = GetComponent<PosFunctions>();
        targetPoint = new Vector3();
        targetReached = false;
        attacked = false;
    }
    public void SetTargetRig(Transform trans) { this.targetTrasform = trans; }
    public Vector3 GetTargetPoint() { return targetPoint; }

    public void AttackAttempt(Transform targetTrans, GameObject targetGO, bool attacked)
    {
        this.targetTrasform = targetTrans;
        this.targetPoint = transform.position;
        this.targetGameObject = targetGO;
        this.attacked = attacked;
    }

    public override bool Condition(Player.Actions state)
    {
        if(attacked)
            return true;

        if (movingToPoint)
            return true;

        if (targetTrasform != null &&
            targetTrasform.GetComponent<MoveToSide>() != null &&
            targetTrasform.GetComponent<MoveToSide>().movingToPoint)
        {
            return true;
        }
            
        Controls controls = GetComponent<Player>().GetControls();
        if ((Input.GetKeyDown(controls.GetPickKey()) || Input.GetKeyDown(controls.GetPicKJoyStick())) 
            && state != Player.Actions.HOLDBUILDING
            && state != Player.Actions.ATTACK)
        {
            TargetFunctions bf = GetComponent<TargetFunctions>();
            if (bf.FindTargetSide(pickUpRange, out targetGameObject, out targetPoint))
            {
                if(targetGameObject.tag == "Player")               
                    targetGameObject.GetComponent<MoveToSide>().AttackAttempt(gameObject.transform, gameObject, true);  // should pass target point

                if (targetGameObject != null && targetTrasform != null)                
                    return true;                
            }
        }
        return false;
    }

    public override void ExcecuteBehaviour()
    {
        if (!movingToPoint && !targetReached)
        {
            targetTrasform.gameObject.isStatic = false;
            StartCoroutine(BehaviourCorutine());
        }
    }

    public IEnumerator BehaviourCorutine()
    {
        targetReached = false;
        movingToPoint = true;

        // Init lerp var
        float timeToDest = 0.0f, currentTime, timeToRotate, timeDiff, timeToMove;
        Quaternion startRot, endRot = Quaternion.identity;
        Vector3 startPos, endPos, dist;

        currentTime = 0.0f;
        startPos = transform.position;
        endPos = new Vector3(targetPoint.x, transform.position.y, targetPoint.z);
        startRot = transform.rotation;

        if (!attacked)
        {
            endRot = Quaternion.LookRotation((targetTrasform.position - endPos).normalized);
            timeToMove = (endPos - transform.position).magnitude / distPerSec;

            Vector3 rotationDiff = new Vector3(endRot.x - transform.rotation.x, endRot.y - transform.rotation.z, endRot.z - transform.rotation.z);
            timeToRotate = rotationDiff.magnitude / rotPerSec;
            timeDiff = Mathf.Abs(timeToRotate - timeToMove);

            timeToDest = (timeToRotate > timeToMove ? timeToMove : timeToRotate) + timeDiff;
        }
        else
        {
            Vector3 enemyTarPoint = targetGameObject.GetComponent<MoveToSide>().targetPoint;
            endRot = Quaternion.LookRotation(new Vector3(enemyTarPoint.x, transform.position.y, enemyTarPoint.z) - transform.position).normalized;

            Vector3 rotationDiff = new Vector3(endRot.x - transform.rotation.x, endRot.y - transform.rotation.z, endRot.z - transform.rotation.z);
            timeToRotate = rotationDiff.magnitude / rotPerSec;

            timeToDest = timeToRotate;
            Debug.DrawLine(enemyTarPoint, transform.position);
        }

        while (currentTime <= timeToDest)
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, currentTime / timeToDest);
            transform.rotation = Quaternion.Lerp(startRot, endRot, currentTime / timeToDest);

            yield return null;
        }

        transform.position = endPos;
        transform.rotation = endRot;
        targetReached = true;

        attacked = false;
        movingToPoint = false;
    }

    public void ResetData()
    {
        //targetGameObject = null;
        targetTrasform = null;
        targetReached = false;
        attacked = false;
    }

    public bool GetTargetReached() { return targetReached; }
    public GameObject GetTarget() { return targetGameObject; }
    public Transform GetTargetTrans(){ return targetTrasform; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);

        if (Application.isPlaying)
        {
            if (targetPoint != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(rotationPoint, 0.1f);
                Gizmos.DrawLine(transform.position, targetPoint);
            }
        }
    }
}