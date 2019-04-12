using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaScript : MonoBehaviour
{
    [SerializeField] private Material[] mat;
    [SerializeField] private float delay;
    [SerializeField] private float transparencyTime;

    private List<Transform> debrisTrans;

    private void OnEnable()
    {
        debrisTrans = new List<Transform>();
        BuildingStats.OnBuildingDebrisTrans += AddDebris; 
    }

    private void AddDebris(List<Transform> debris)
    {
        foreach (Transform t in debris)
        {
            if (ErrorCheck(t.gameObject))
            {
                //Destroy(t.gameObject);
                return;
            }
            else
                StartCoroutine(WaitToAdd(t));
        }
    }

    private IEnumerator WaitToAdd(Transform t)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(TransparencyCorutine(0, t.gameObject));
    }

    private IEnumerator TransparencyCorutine(int v, GameObject transpObj)
    {
        float alpha = 0;
        float endAlpha = 0.0f;
        float timer = 0;
        Color color = transpObj.GetComponent<MeshRenderer>().material.color;
        alpha = color.a;

        transpObj.GetComponent<MeshRenderer>().materials = mat;

        while (alpha >= 0)
        {
            timer += Time.deltaTime;
            if (timer >= transparencyTime)
            {
                break;
            }
            alpha = Lerp(1.0f, endAlpha, timer, transparencyTime);
            if (transpObj != null)
                transpObj.GetComponent<MeshRenderer>().materials[0].color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }
        if(transpObj != null)
            transpObj.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, color.b, endAlpha);

        Destroy(transpObj);
    }

    private float Lerp(float start, float end, float currentTime, float totalTime)
    {
        return start - (currentTime / totalTime) * (start - end);
    }

    private bool ErrorCheck(GameObject transpObj)
    {
        if (transpObj.GetComponent<MeshRenderer>() == null ||
            transpObj.GetComponent<MeshRenderer>().materials.Length != 0)
        {
            return false;
        }
        return true;
    }

    public float GetDelay() { return delay; }
    public float GetTransparentRate() { return transparencyTime; }
}