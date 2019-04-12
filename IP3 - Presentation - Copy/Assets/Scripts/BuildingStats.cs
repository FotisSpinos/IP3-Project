using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(TargetPointGen))]
[RequireComponent(typeof(BoxCollider))]

public class BuildingStats : MonoBehaviour
{
    [SerializeField] private byte maxDurability;
    private byte currentDurability;
    public bool thrown = false;
    public GameObject playerPick;
    private int buildingScore;

    private Dictionary<int, List<Transform>> levelDictionary;

    private Transform[] allDebris; 

    private string[] throwInteractionTags;
    
    public enum BuildingType
    {
        BUILDING,
        GENERATOR
    }

    [SerializeField] BuildingType buildingType;

    public delegate void BuildingParticleSpawn(Vector3 pos, BuildingType buildingType);
    public static event BuildingParticleSpawn OnBuildingDestroy;

    public delegate void BuildingDebrisTrans(List<Transform> debris);
    public static event BuildingDebrisTrans OnBuildingDebrisTrans;

    public delegate void ScoreUpdate(int playerIndex, int maxDurability);
    public static event ScoreUpdate OnBuildingScoreUpdate;

    public delegate void PlaySound(AudioManager.BuildingEvent be, Vector3 pos);
    public static event PlaySound OnPlaySound;


    private int destroyedDebrisCounter;
    [SerializeField] private bool boostBuilding;

    private void Start ()
    {
        levelDictionary = new Dictionary<int, List<Transform>>();   //0 -> top, 1 -> middle, 2 -> botom
        throwInteractionTags = new string[] { "Building", "ground", "Border", "Player" , "Top", "Middle", "Bottom", "Decoration"};

        buildingType = BuildingType.BUILDING;

        currentDurability = maxDurability;
        allDebris = GetComponentsInChildren<Transform>();
        DefineBuildingLevels();
        buildingScore = maxDurability;
    }

    private void DefineBuildingLevels() // separate building into sections
    {
        List<Transform> top = new List<Transform>();
        List<Transform> middle = new List<Transform>();
        List<Transform> bottom = new List<Transform>();

        levelDictionary.Add(0, bottom);
        levelDictionary.Add(1, middle);
        levelDictionary.Add(2, top);

        for (int i = 0; i < allDebris.Length; i++)
        {
            if (!ValidDebris(allDebris[i].gameObject))
            {
                continue; 
            }

            if (allDebris[i].gameObject.tag == "Top")
            {
                top.Add(allDebris[i]);
            }
            else if (allDebris[i].gameObject.tag == "Middle")
            {
                middle.Add(allDebris[i]);
            }
            else if (allDebris[i].gameObject.tag == "Bottom")
            {
                bottom.Add(allDebris[i]);
            }           
            else
            {
                if (ValidDebris(allDebris[i].gameObject) && allDebris[i].tag != "Building")
                {
                    allDebris[i].gameObject.tag = "bottom";
                    bottom.Add(allDebris[i]);
                }
            }         
        }
    }

    private bool ValidDebris(GameObject debris)
    {
        if (debris == null ||
            debris.GetComponent<MeshRenderer>() == null ||
            debris.GetComponent<MeshRenderer>().material == null ||
            debris.GetComponent<Rigidbody>() == null || 
            debris.GetComponent<BoxCollider>() == null)
        {
            return false;
        }
        return true;
    }

    public void SetPlayerPick(GameObject obj) { playerPick = obj; }
    private bool collided = false;

    private void OnCollisionEnter(Collision other)
    {
        if (!thrown)
        {
            if (other.gameObject.tag == "Player")
            {
                Damage(currentDurability, other.gameObject.GetComponent<MoveToSide>().GetTarget().transform);
            }
            else if (other.gameObject.tag == "Building" || other.gameObject.transform.root.tag == "Building")
            {
                Damage(1, other.gameObject.GetComponent<BuildingStats>().playerPick.transform);
            }
        }
        if (thrown)
        {
            for (int i = 0; i < throwInteractionTags.Length; i++)
            {
                if (other.gameObject.tag == throwInteractionTags[i] && other.gameObject != playerPick && !collided)
                {
                    collided = true;
                    //if (other != null && playerPick.GetComponent<Player>())
                    //    OnBuildingScoreUpdate(playerPick.GetComponent<Player>().GetPlayerIndex(), maxDurability);
                    Damage(currentDurability, playerPick.transform);
                }
            }
        }
    }

    private void DestroyBuilding()
    {
        for(int i = currentDurability - 1; i >= 0; i--)
        {
            DestroyLevel(i);
        }
    }

    private void BuildingTremble(Vector3 playerPos)
    {
        Vector3 dir = (transform.localPosition - playerPos).normalized;
        Vector3 destPoint = transform.position + dir * 0.1f;

        StartCoroutine(MoveToPoint(destPoint));
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        float distFraction = 0;
        float dist = (transform.position - point).magnitude;
        Vector3 startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 endPos = new Vector3(point.x, transform.position.y, point.z);
        float startTime = Time.time;

        for(int i = 0; i < 4; i++)
        {
            while (distFraction < 1)
            {
                float distCovered = (Time.time - startTime) * 2f;
                distFraction = distCovered / dist;

                transform.position = Vector3.Lerp(startPos, endPos, distFraction);
                yield return null;
            }

            endPos = startPos;
            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            startTime = Time.time;
            distFraction = 0;
        }
    }

    private void DestroyLevel(int durability)
    {
        foreach (Transform debris in levelDictionary[durability])
        {
            if (debris != null)
            {
                debris.parent = null;
                debris.gameObject.isStatic = false;
                debris.GetComponent<Collider>().enabled = true;
                debris.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        OnBuildingDebrisTrans(levelDictionary[durability]);

        if (durability == 0)
            GetComponent<BoxCollider>().enabled = false;
    }

    public void Damage(byte damage, Transform attackerTrans)
    {
        byte updatedDurability = 0;

        if (currentDurability - damage <= 0)
        {
            OnPlaySound(AudioManager.BuildingEvent.DESTROY, transform.position);

            if (boostBuilding && attackerTrans != null)
                attackerTrans.GetComponent<BoostMode>().PlayerBoost();

            if (attackerTrans != null && attackerTrans.GetComponent<Player>())           
                OnBuildingScoreUpdate(attackerTrans.GetComponent<Player>().GetPlayerIndex(), maxDurability);
            
            else Debug.Log("PLAYER DOESNT EXIST" + gameObject.name);

            OnBuildingDestroy(transform.position, buildingType);
            DestroyBuilding();
            return;
        }

        OnPlaySound(AudioManager.BuildingEvent.DAMAGE, transform.position);
        for (int i = 1; i <= damage; i++)
        {
            updatedDurability = (byte)(currentDurability - i);
            OnBuildingDebrisTrans(levelDictionary[updatedDurability]);
            DestroyLevel(updatedDurability);

            if (attackerTrans != null)
                BuildingTremble(attackerTrans.position);
        }

        currentDurability = updatedDurability;
    }

    public float GetCurrentDurability() { return currentDurability; }
    public int GetBuildingScore() { return buildingScore; }
    public bool isThrown() { return thrown; }
}