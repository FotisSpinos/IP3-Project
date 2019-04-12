using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsDestroy : MonoBehaviour
{
    private Transform[] allDebris;
    private List<Transform> top = new List<Transform>();
    private List<Transform> middle = new List<Transform>();
    private List<Transform> bottom = new List<Transform>();
    private AlphaScript at;

    private void DefineBuildingLevels() // separate building into sections
    {
        for (int i = 0; i < allDebris.Length; i++)
        {
            if (allDebris[i] == null)
            {
                continue;
            }

            if (allDebris[i] != this.transform && allDebris[i].GetComponent<Collider>() != null)
                allDebris[i].GetComponent<Collider>().enabled = false;

            if (allDebris[i].gameObject.tag == "top")
            {
                top.Add(allDebris[i]);
            }
            else if (allDebris[i].gameObject.tag == "middle")
            {
                middle.Add(allDebris[i]);
            }
            else if (allDebris[i].gameObject.tag == "bottom")
            {
                bottom.Add(allDebris[i]);
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        bool thrown = true;
        if (thrown == true)
        {
            if (other.gameObject.tag == "Building" ||
                other.gameObject.tag == "ground" ||
                other.gameObject.tag == "Border" ||
                other.gameObject.tag == "top" ||
                other.gameObject.tag == "middle" ||
                other.gameObject.tag == "bottom" ||
                other.gameObject.tag == "Player")
            {
                DestroyLevel(bottom);
            }
        }
        else
            if (other.gameObject.tag == "Player")
        {
            DestroyLevel(bottom);
        }
    }

    private void DestroyLevel(List<Transform> currentLevel)
    {
        SeperateLevel(currentLevel);
        StartCoroutine(DestroyLevelObjects(currentLevel, 1.5f));
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, 5);
    }

    private void SeperateLevel(List<Transform> buildingLevel)
    {
        foreach (Transform debris in buildingLevel)
        {
            if (debris != null)
            {
                debris.parent = null;
                debris.GetComponent<Collider>().enabled = true;
                debris.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        //at.DecreaseTransparency(0.01f, buildingLevel);
    }

    private IEnumerator DestroyLevelObjects(List<Transform> currentLevel, float time)
    {
        yield return new WaitForSeconds(time);

        foreach (Transform level in currentLevel)
        {
            if (level != null)
            {
                //Destroy(level.gameObject);
            }
        }
    }
}
