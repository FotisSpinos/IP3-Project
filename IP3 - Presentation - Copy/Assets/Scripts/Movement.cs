using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController cc;
    private Controls controls;
    private CharacterGravity cg;

    protected void Start()
    {
        controls = GetComponent<Player>().GetControls();
        cg = GetComponent<CharacterGravity>();
        cc = GetComponent<CharacterController>();
    }


    public void MoveForward(float speed)
    {
        Vector3 moveVector = transform.forward * speed * Time.deltaTime;
        cc.Move(new Vector3(moveVector.x, 0, moveVector.z));
    }

    public void Rotate(float speed)
    {
        Quaternion rotation = transform.rotation;
        float y = rotation.eulerAngles.y;

        y -= -Input.GetAxisRaw(controls.GetHorizontalInput()) * speed;
        transform.rotation = transform.rotation = Quaternion.Euler(0, y, 0);
    }
}