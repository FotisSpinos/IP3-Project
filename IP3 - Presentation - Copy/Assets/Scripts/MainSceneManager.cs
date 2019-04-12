using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField] private float timeToLoadEndScene;

    private int destroyedBuildings;
    private int buildingsInSceen;

    [SerializeField] private float gameStartPauseTimer;

    private MainSceneUIManager uiManager;
    public delegate void GameStartPause(bool gameStartPause);
    public static event GameStartPause OnGameStartPause;

    private void OnEnable()
    {
        BuildingStats.OnBuildingDestroy += DestroyedBuilding;
    }

    private void Start()
    {
        if(GetComponent<MainSceneUIManager>() != null)
            uiManager = GetComponent<MainSceneUIManager>();
        else Debug.Log("UIManager was not attached to the game Object");

        StartCoroutine(LevelOpenDelay());

        buildingsInSceen = GameObject.FindGameObjectsWithTag("Building").Length;
        timeToLoadEndScene++;
        uiManager.SetUpUI((int)(timeToLoadEndScene / 60));
    }

    private void DestroyedBuilding(Vector3 pos, BuildingStats.BuildingType type)
    {
        destroyedBuildings++;

        if (destroyedBuildings == buildingsInSceen)
            LoadEndScene();
    }

    private float lastTime;
    private float updatedTime;

    private void LoadEndScene()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("EndScene");
    }

    private IEnumerator WaitToLoadEndScene()
    {
        float timer = timeToLoadEndScene;
        int lastTime = 0, newTime = 0;

        while(timer >= 0)
        {
            lastTime = (int)timer;
            timer -= Time.deltaTime;
            newTime = (int)timer;

            yield return null;

            if (lastTime != newTime)
                uiManager.LevelTimerUpdate(newTime);
        }
        LoadEndScene();
    }

    private IEnumerator LevelOpenDelay()
    {
        OnGameStartPause(true);
        float timer = gameStartPauseTimer;
        int lastTime = 0, newTime = 0;

        while (timer > 0)
        {
            lastTime = (int)timer;
            timer -= Time.deltaTime;
            newTime = (int)timer;
            yield return null;

            if (lastTime != newTime)
                uiManager.CountDownTimerUpdate(newTime);
        }
        uiManager.CountDownTimerUpdate(-1);
        OnGameStartPause(false);

        StartCoroutine(uiManager.WaitToDestroyCoundownObj());
        StartCoroutine(WaitToLoadEndScene());
    }
}
