using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int score;

    public void GainScore(int score) { score += score; }
    public int GetScore() { return score; }
}
