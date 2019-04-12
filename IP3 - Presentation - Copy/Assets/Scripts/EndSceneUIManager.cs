using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneUIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] winSprites;
    [SerializeField] private GameObject winObject;

    public void DisplayWinSprite(string playerName)
    {
        if (winObject.GetComponent<Sprite>() || winSprites == null || winSprites.Length < 4)
            return;

        Sprite selectedSprite = null;

        switch(playerName)
        {
            case "Spiderbeak":
                selectedSprite = winSprites[0];
                break;

            case "Centipede":
                selectedSprite = winSprites[1];
                break;

            case "Lobster":
                selectedSprite = winSprites[2];
                break;

            case "Bulltoad":
                selectedSprite = winSprites[3];
                break;
            default:
                Debug.Log(playerName + "Is not recognised!");
                selectedSprite = winSprites[0];
                break;
        }
        winObject.GetComponent<Image>().sprite = selectedSprite;
    }
}
