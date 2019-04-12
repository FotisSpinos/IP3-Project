using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosFunctions : MonoBehaviour
{
    public float AngleFactor(Vector3 target)
    {
        return AngleXZ(target) / 500;
    }

    public float AngleXZ(Vector3 target)
    {
        Vector3 objDir = new Vector3(transform.forward.x, 0, transform.forward.z);
        Vector3 buildingDir = new Vector3(target.x, 0, target.z) - new Vector3(transform.localPosition.x, 0, transform.localPosition.z);

        return Vector3.Angle(objDir, buildingDir);
    }

    public float AnglePlusDist(Vector3 target)
    {
        return AngleFactor(target) + DistanceMagnitudeXZ(target);
    }

    public float DistanceMagnitudeXZ(Vector3 target)
    {
        Vector3 objPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 buildingPos = new Vector3(target.x, 0, target.z);
        
        return (buildingPos - objPos).magnitude;
    }

    public void MoveTowards(Vector3 target, float speed, float maxDistError)
    {
        if (maxDistError > DistanceMagnitudeXZ(target))
            return;

        Vector3 direction = (target - transform.position).normalized;
        transform.position += (new Vector3(direction.x, 0, direction.z) * speed);
    }

    public void RotateTowardsXZ(Vector3 target, float speed, float maxAngError)
    {
        if (maxAngError > AngleXZ(target))
            return;

        Vector3 rotationDir = (target - transform.localPosition).normalized;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, rotationDir, speed, 0.0f);
        transform.localRotation = Quaternion.LookRotation(new Vector3(newDir.x, 0, newDir.z));
    }
}
