using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointGen : MonoBehaviour
{
    private Vector3 objSize;
    private List<Vector3> points;
    private Vector3[] directions;
    [SerializeField] private float offset;

    public List<Vector3> GetGenPoints()
    {
        return points;
    }

    private void Start()
    {
        if (offset == 0)
            offset = 0.3f;

        points = new List<Vector3>();
        if (GetComponent<BoxCollider>())
            objSize = GetComponent<BoxCollider>().bounds.size;

        if(GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().isKinematic = true;

        directions = new Vector3[4];
        directions[0] = -transform.right;
        directions[1] = transform.right;
        directions[2] = transform.forward;
        directions[3] = -transform.forward;

        //Use For Testing. Remove On Release
        SetPoints(GameObject.Find("Centipede Player1").GetComponent<CharacterController>().bounds.size, GameObject.Find("Centipede Player1").GetComponent<CharacterController>().radius);
    }

    public void SetPoints(Vector3 playerSize, float radious)
    {
        points.Clear();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 currentPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z) + new Vector3((objSize.x / 2 + playerSize.x / 2) * directions[i].x, 0,
            (objSize.z / 2 + playerSize.z / 2) * directions[i].z) + new Vector3(directions[i].x * offset, 0, directions[i].z * offset);

            Collider[] colliders = Physics.OverlapSphere(currentPoint, new Vector3(playerSize.x, transform.position.y, playerSize.z).magnitude / 2.0f);
            bool validPoint = true;

            
            for(int c = 0; c < colliders.Length; c++)
            {
                if(colliders[c].tag == "Building" || colliders[c].tag == "Border")
                {
                    validPoint = false;
                }
            }
                        
            if (!validPoint)
            {
                points.Add(currentPoint);
            }
        }
    }

    public bool isBuildingToPoint(Vector3 point)
    {
        Collider[] colliders = Physics.OverlapSphere(point, 0.2f);

        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Building")
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(point, 0.2f);
                Gizmos.DrawLine(point, transform.position);
            }
        }
    }
}