using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Walk : CharacterBehaviour
{
    [Header("Speed Variables")]
    [SerializeField] public float moveSpeed;
    [SerializeField] private float rotSpeed;
    private Movement movement;
    private Controls controls;

    public void Start()
    {
        movement = GetComponent<Movement>();
        if (movement == null)
            movement = gameObject.AddComponent<Movement>();
        controls = GetComponent<Controls>();
    }
    public float GetMoveSpeed() { return moveSpeed; }

    public override void ExcecuteBehaviour()
    {
        float inputAxis = Input.GetAxis(GetComponent<Controls>().GetVerticalInput());
        movement.MoveForward(moveSpeed * inputAxis);
        movement.Rotate(rotSpeed);
    }

    public override bool Condition(Player.Actions state)
    {
        if (state == Player.Actions.ATTACK)
            return false;

        Controls controls = GetComponent<Player>().GetControls();

        if (Mathf.Abs(Input.GetAxis(controls.GetHorizontalInput())) > 0
            || Mathf.Abs(Input.GetAxis(controls.GetVerticalInput())) > 0)        
            return true;        
        return false;
    }
}