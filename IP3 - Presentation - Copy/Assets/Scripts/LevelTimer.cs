using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTimer : MonoBehaviour {

    public Text timerText;
    public float resetTimer;
    public Transform win;
    public Transform end;
    //public Transform endPanel;
    private float sec;
    private float minutes;

    IEnumerator ResetGame()
    {
       yield return new WaitForSeconds(2f);
       // SceneManager.LoadScene("SempleScene");

    }
    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
        resetTimer = 0;
        sec = 60 - 1;
        minutes = 1;
         timerText.text = "0"+ minutes + " : " + sec;
    }

    // Update is called once per frame
    void Update()
    {
        resetTimer += Time.deltaTime;
        if(resetTimer >= 60)
        {
            StartCoroutine(ResetGame());
        }
        Timer();
        Test();
    }
    void Timer()
    {
        if (sec > 0)
            sec -= Time.deltaTime;
        if (sec <= 0 && minutes != 0)
        {
            minutes--;
            sec = 60;

        }
        if(sec >= 0 && sec <= 9)
        {
            timerText.text = "0" + minutes + " : " + "0" + sec.ToString("f0");
        }
        else
        timerText.text = "0" + minutes + " : " + sec.ToString("f0");
        if (sec <= 0 && minutes == 0)
        {
            timerText.text = "Time's Up!";
            // StartCoroutine(NextScene());
            Time.timeScale = 0;
            //endPanel.gameObject.SetActive(true);
            end.gameObject.SetActive(true);
        }
    }
    // test for no building condition
    void Test()
    {
        if (GameObject.FindWithTag("building") == null)
        {
            Debug.Log("No other buildings in the scene");
            //SceneManager.LoadScene("EndScene");
            Time.timeScale = 0;
            //endPanel.gameObject.SetActive(true);
            win.gameObject.SetActive(true);
        }
    }
 
}
