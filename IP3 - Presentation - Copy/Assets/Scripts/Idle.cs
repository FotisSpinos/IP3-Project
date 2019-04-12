using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : CharacterBehaviour
{
    private bool gameStartPause;

    private void OnEnable()
    {
        MainSceneManager.OnGameStartPause += SetGameStatePause;
    }

    private void Start()
    {
        gameStartPause = false;
    }

    private void SetGameStatePause(bool gameStartPause)
    {
        this.gameStartPause = gameStartPause;
    }

    public override bool Condition(Player.Actions state)
    {
        if (gameStartPause)
            return true;
        return false;
    }

    public override void ExcecuteBehaviour()
    {
        
    }
}
