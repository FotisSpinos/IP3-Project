using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//myriad pro font unity

public class MainSceneUIManager : MonoBehaviour
{
    /*
      playerNames elements Notee!!! BuildingsScore is a parallel array
     0 -> SpiderBeak
     1 -> Centipede
     2 -> Lobster
     3 -> Toad
     */

    private int[] playerScore;
    [SerializeField] private string[] playerNames;
    [SerializeField] private int[] buildingsScore;
    [SerializeField] private GameObject[] playerModels;


    [SerializeField] private Text[] scoresUI;
    [SerializeField] private Text levelTimer;
    [SerializeField] private Sprite[] countdownSprites;
    [SerializeField] private float destroyUITimer;
    [SerializeField] private GameObject countDownObject;
    private Image countDownImage;

    private void OnEnable()
    {        
        BuildingStats.OnBuildingScoreUpdate += ScoreUpdate;
    }

    private void Start()
    {
        playerScore = new int[4];

        if (countDownObject.GetComponent<Image>() != null)
            countDownImage = countDownObject.GetComponent<Image>();
        else Debug.Log("CountDownObject does not have an Image component attached");
    }

    public void SetUpUI(int timeToLoadEndScene)
    {
        levelTimer.text = "0" + timeToLoadEndScene.ToString() + " : 00";

        for(int i = 0; i < scoresUI.Length; i++)
        {
            scoresUI[i].text = "0";
        }
    }

    public void ScoreUpdate(int playerIndex, int maxDurability)
    {
        maxDurability--;
        playerScore[playerIndex] += buildingsScore[maxDurability];  //out of range for some reaons 

        if (scoresUI[playerIndex] != null)
        {
            scoresUI[playerIndex].text = playerScore[playerIndex].ToString();
        }
    }

    public void CountDownTimerUpdate(int timer)
    {
        countDownImage.sprite = countdownSprites[timer + 1];
    }

    public IEnumerator WaitToDestroyCoundownObj()
    {
        yield return new WaitForSeconds(destroyUITimer);
        Destroy(countDownObject);
    }

    public void LevelTimerUpdate(int timer)
    {
        float seconds = 0, minutes = 0;
        
        minutes = (timer / 61 );
        seconds = timer - minutes * 60;
        char[] minutesChar = timeChar(minutes.ToString().ToCharArray());
        char[] secondsChar = timeChar(seconds.ToString().ToCharArray());

        levelTimer.text = minutesChar[0] + "" + minutesChar[1] + " : " + secondsChar[0] + "" + secondsChar[1];
    }

    private char[] timeChar(char[] time)
    {
        if (time.Length < 2)
        {
            char[] newMinChar = new char[2];
            newMinChar[0] = '0';
            newMinChar[1] = time[0];
            return newMinChar;
        }
        return time;
    }

    public int[] GetPlayerScore() { return playerScore; }
    public GameObject[] GetPlayerModels() { return playerModels; }
    public string[] GetPlayerNames() { return playerNames; }
}