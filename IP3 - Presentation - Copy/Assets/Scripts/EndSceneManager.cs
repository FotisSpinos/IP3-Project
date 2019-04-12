using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnCubes;
    [SerializeField] private Transform[] rotationTransforms;
    private Vector3[] spawnPoints;

    private int[] playerScores;
    [SerializeField] private GameObject[] playerModels;
    [SerializeField] private string[] playerNames;

    private EndSceneUIManager esuim;
    public string[] GetPlayerNames() { return playerNames; }

    private void OnEnable()
    {
        spawnPoints = new Vector3[4];
        esuim = GetComponent<EndSceneUIManager>();

        GetMainSceneData();

        DefineSpawnPoints();
        InserionSort();
        SpawnCharacters();
        DisplayWinSce();
    }

    private void InserionSort()
    {
        for(int i = 1; i < playerScores.Length; i++)
        {
            int highsetElement = playerScores[i];
            GameObject highestModel = playerModels[i];
            string playerName = playerNames[i];
            int currentIndex = i - 1;

            while(currentIndex >= 0 && playerScores[currentIndex] < highsetElement)
            {
                playerScores[currentIndex + 1] = playerScores[currentIndex];
                playerModels[currentIndex + 1] = playerModels[currentIndex];
                playerNames[currentIndex + 1] = playerNames[currentIndex];
                currentIndex--;
            }

            playerScores[currentIndex + 1] = highsetElement;
            playerModels[currentIndex + 1] = highestModel;
            playerNames[currentIndex + 1] = playerName;
        }
    }

    private void DisplayWinSce()
    {
        esuim.DisplayWinSprite(playerNames[0].ToString());
    }

    private void SpawnCharacters()
    {
        for(int i = 0; i < playerScores.Length; i++)
        {
            Instantiate(playerModels[i]);
            playerModels[i].transform.position = spawnPoints[i];
            playerModels[i].transform.rotation = rotationTransforms[i].rotation;
        }
    }

    private void DefineSpawnPoints()
    {
        if (spawnPoints.Length != spawnCubes.Length)
        {
            Debug.Log("Arrays " + spawnCubes.ToString() + " and " + spawnPoints.ToString() + " Dont have the same amount of elelments");
            return;
        }

        for(int i = 0; i < spawnCubes.Length; i++)
        {
            if (spawnCubes[i].GetComponent<MeshRenderer>() == null)
                spawnCubes[i].AddComponent<MeshRenderer>();
            MeshRenderer ms = spawnCubes[i].GetComponent<MeshRenderer>();

            float verticalSize = ms.bounds.size.y / 2;
            Vector3 spawnObjectPos = spawnCubes[i].transform.position;
            spawnPoints[i] = new Vector3(spawnObjectPos.x, spawnObjectPos.y + verticalSize, spawnObjectPos.z);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (Application.isPlaying && spawnPoints != null)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Gizmos.DrawSphere(spawnPoints[i], 0.2f);
            }
        }
    }

    
    private void GetMainSceneData()
    {
        GameObject mainSceneMnr = GameObject.Find("SceneManager");
        if (mainSceneMnr == null)
            return;

        MainSceneUIManager msuim = mainSceneMnr.GetComponent<MainSceneUIManager>();
        playerScores = msuim.GetPlayerScore();
        playerModels = msuim.GetPlayerModels();
        playerNames = msuim.GetPlayerNames();

    }
}