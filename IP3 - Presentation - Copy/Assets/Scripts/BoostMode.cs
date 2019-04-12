using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMode : MonoBehaviour
{
    [SerializeField] private float boostDuration;

    [SerializeField] private float boostDashSpeed;
    [SerializeField] private float boostWalkSpeed;
    [SerializeField] private byte boostDamage;

    float dashspeed;
    float walkSpeed;
    byte damage;

    private Walk w;
    private Dash d;
    private Attack a;

    private void Start()
    {
        //Set default varialbes
        w = GetComponent<Walk>();
        d = GetComponent<Dash>();
        a = GetComponent<Attack>();

        dashspeed = d.moveSpeed;
        walkSpeed = w.moveSpeed;
        damage = a.damage;
    }

    public void PlayerBoost()
    {
        StartCoroutine(PlayerBoostCorutine());
    }

    private IEnumerator PlayerBoostCorutine()
    {
        ApplyBoostVariables();
        yield return new WaitForSeconds(boostDuration);

        DetatchBoostVariables();
    }

    private void ApplyBoostVariables()
    {
        d.moveSpeed += boostDashSpeed;
        w.moveSpeed += boostWalkSpeed;
        a.damage += boostDamage;
    }

    private void DetatchBoostVariables()
    {
        d.moveSpeed -= boostDashSpeed;
        w.moveSpeed -= boostWalkSpeed;
        a.damage -= boostDamage;
    }
}
