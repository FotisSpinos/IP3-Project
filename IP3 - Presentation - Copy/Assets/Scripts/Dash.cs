using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : CharacterBehaviour
{
    [Header("Speed Variables")]
    [SerializeField] public float moveSpeed;
    [SerializeField] private float rotSpeed;

    [Header("Cooldown Variable")]
    [SerializeField] private float dashCooldown;

    private bool canDash;
    private bool dashing;
    private Movement movement;

    public float GetMoveSpeed() { return moveSpeed; }

    public void Start()
    {
        dashing = false;
        canDash = true;
        if (GetComponent<Movement>())
            movement = GetComponent<Movement>();
        else movement = gameObject.AddComponent<Movement>();
    }

    public override void ExcecuteBehaviour()
    {
        if (canDash)
        {
            StartCoroutine(DashCorutine());
        }
        else if(dashing)
        {
            movement.MoveForward(moveSpeed);
            movement.Rotate(rotSpeed);
        }
    }

    private IEnumerator DashCorutine()
    {
        canDash = false;
        dashing = true;
        yield return new WaitForSeconds(2.3f);
        dashing = false;
        StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public override bool Condition(Player.Actions state)
    {
        if (state == Player.Actions.ATTACK)
            return false;

        if (dashing)
            return true;

        Controls controls = GetComponent<Player>().GetControls();
            if (Input.GetKey(controls.GetKeyDash()) || Input.GetKey(controls.GetJoyStickDash()))
                if(canDash)
                    return true;
        return false;
    }
}
