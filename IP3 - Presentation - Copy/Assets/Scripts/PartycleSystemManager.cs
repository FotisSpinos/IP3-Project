using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartycleSystemManager : MonoBehaviour
{
    [SerializeField] GameObject[] buildingParticles;

    private void OnEnable()
    {
        BuildingStats.OnBuildingDestroy += BuildingDestroy;
    }

    private void BuildingDestroy(Vector3 pos, BuildingStats.BuildingType buildingType)   //Spawn particle on Destroyed Building
    {
        GameObject selectedParticle = null;

        if (buildingType == BuildingStats.BuildingType.BUILDING)
            selectedParticle = buildingParticles[0];
        else if (buildingType == BuildingStats.BuildingType.GENERATOR)
            selectedParticle = buildingParticles[1];

        GameObject spawnParticle = Instantiate(selectedParticle);
        spawnParticle.transform.position = pos;
        ParticleSystem ps = spawnParticle.GetComponent<ParticleSystem>();
        ps.Play();

        StartCoroutine(DestroyParticle(spawnParticle, ps));
    }

    private IEnumerator DestroyParticle(GameObject spawnParticle, ParticleSystem ps)
    {
        yield return new WaitForSeconds(ps.duration);
        Destroy(spawnParticle);
    }
}
