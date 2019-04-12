using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFunctions : MonoBehaviour
{
    private List<Collider> validTargets;
    private PosFunctions posFunct;

    public void Initialize()
    {
        validTargets = new List<Collider>();
        posFunct = GetComponent<PosFunctions>();
    }

    public void FindValidTargets(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && 
                IsValidTarget(colliders[i].gameObject))
            {
                validTargets.Add(colliders[i]);
            }
        }
    }

    private bool IsValidTarget(GameObject target)
    {

        if (!GetComponent<Player>())
        {
            Debug.Log("Component Player is missing");
            return false;
        }
        if(target == null)
        {
            Debug.Log("Parameter target is not set to an instance of an object");
            return false;
        }
        Player.Actions state = GetComponent<Player>().GetState();

        if ((target.tag == "Building") && target.gameObject.GetComponent<TargetPointGen>().enabled)
        {
            if (state == Player.Actions.ATTACK)           
                return true;
            
            else if (target.gameObject.GetComponent<BuildingStats>().GetCurrentDurability() == 1)
                    return true;
        }

        if (target.tag == "Player")
        {
            if (!target.GetComponent<CapsuleCollider>())
                return false;

            CapsuleCollider cc = target.GetComponent<CapsuleCollider>();

            if (target.GetComponent<MoveToSide>().GetTargetTrans() == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool FindTargetSide(float pickUpRange, out GameObject selectedBuilding, out Vector3 targetPoint)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickUpRange);

        if (FindTargetsInRange(colliders).Count > 0)
        {
            selectedBuilding = SelectBuildingToPick(90);
            if (selectedBuilding != null &&
                selectedBuilding.GetComponent<TargetPointGen>() != null)
            {
                selectedBuilding.GetComponent<TargetPointGen>().SetPoints(GetComponent<CapsuleCollider>().bounds.size, GetComponent<CapsuleCollider>().radius); // no set to a reference of an object
                targetPoint = SelectTargetSide(selectedBuilding.GetComponent<TargetPointGen>());

                if (targetPoint != Vector3.zero)
                {
                    MoveToSide monsterController = GetComponent<MoveToSide>();
                    monsterController.SetTargetRig(selectedBuilding.GetComponent<Transform>());
                    return true;
                }
            }
            else
                targetPoint = Vector3.zero;
        }
        else
        {
            selectedBuilding = null;
            targetPoint = Vector3.zero;
        }
        return false;
    }

    public GameObject SelectBuildingToPick(float angleError)
    {
        if (validTargets.Count > 0)
        {
            float minAnglePlusDist = float.PositiveInfinity;
            Collider closestBuidling = null;

            foreach (Collider c in validTargets)
            {
                float currentAnglePlusDist = posFunct.AnglePlusDist(c.transform.position);
                float angle = posFunct.AngleXZ(c.transform.localPosition);

                if (angle > angleError)
                    continue;

                if (currentAnglePlusDist <= minAnglePlusDist)
                {
                    minAnglePlusDist = currentAnglePlusDist;
                    closestBuidling = c;
                }
            }
            return closestBuidling.gameObject;
        }
        return null;
    }

    public Vector3 SelectTargetSide(TargetPointGen building)
    {
        if (building.GetComponent<TargetPointGen>().GetGenPoints().Count > 0)
        {
            float minDistance = (building.GetGenPoints()[0] - transform.position).magnitude;
            Vector3 targetPoint = building.GetGenPoints()[0];

            foreach (Vector3 point in building.GetGenPoints())
            {
                if ((point - transform.position).magnitude < minDistance)
                {
                    minDistance = (point - transform.position).magnitude;
                    targetPoint = point;
                }
            }
            return targetPoint;
        }
        return Vector3.zero;
    }

    public bool isValidTarget(Collider building, float visionRange)
    {
        if (posFunct.AngleFactor(building.transform.position) < visionRange)
        {
            return true;
        }
        return false;
    }

    public List<Collider> FindTargetsInRange(Collider[] colliders)
    {
        if (validTargets != null)
        {
            validTargets.Clear();
            FindValidTargets(colliders);
        }
        return validTargets;
    }
}
