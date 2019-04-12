using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    private KeyCode keyAttack;
    private KeyCode keyPickUp;
    private KeyCode keyDash;
    private KeyCode keyThrow;
    private KeyCode joyStickPickUp;
    private KeyCode joyStickAttack;
    private KeyCode joyStickDash;
    private KeyCode joyStickThrow;

    private string horizontalInput;
    private string verticalInput;

    public void SetControlls(int playerIndex)
    {
        switch (playerIndex)
        {
            case 1:
                keyAttack = KeyCode.Q;
                keyPickUp = KeyCode.E;
                keyDash = KeyCode.X;
                joyStickPickUp = KeyCode.Joystick1Button0;
                joyStickAttack = KeyCode.Joystick1Button1;
                joyStickDash = KeyCode.Joystick1Button2;
                horizontalInput = "PLAYER_" + playerIndex + "Horizontal";
                verticalInput = "PLAYER_" + playerIndex + "Vertical";

                break;

            case 2:
                keyAttack = KeyCode.U;
                keyPickUp = KeyCode.O;
                keyDash = KeyCode.M;
                joyStickPickUp = KeyCode.Joystick2Button0;
                joyStickAttack = KeyCode.Joystick2Button1;
                joyStickDash = KeyCode.Joystick2Button2;
                horizontalInput = "PLAYER_" + playerIndex + "Horizontal";
                verticalInput = "PLAYER_" + playerIndex + "Vertical";
                break;

            case 3:
                keyAttack = KeyCode.V;
                keyPickUp = KeyCode.C;
                keyDash = KeyCode.Z;
                joyStickPickUp = KeyCode.Joystick3Button0;
                joyStickAttack = KeyCode.Joystick3Button1;
                joyStickDash = KeyCode.Joystick3Button2;
                horizontalInput = "PLAYER_" + playerIndex + "Horizontal";
                verticalInput = "PLAYER_" + playerIndex + "Vertical";
                break;

            case 4:
                keyAttack = KeyCode.B;
                keyPickUp = KeyCode.N;
                keyDash = KeyCode.LeftShift;
                joyStickPickUp = KeyCode.Joystick4Button0;
                joyStickAttack = KeyCode.Joystick4Button1;
                joyStickDash = KeyCode.Joystick4Button2;
                horizontalInput = "PLAYER_" + playerIndex + "Horizontal";
                verticalInput = "PLAYER_" + playerIndex + "Vertical";
                break;
        }
    }

    // Getters
    public KeyCode GetAttackKey() { return keyAttack; }
    public KeyCode GetAttackJoyStick() { return joyStickAttack; }
    public KeyCode GetPickKey() { return keyPickUp; }
    public KeyCode GetPicKJoyStick() { return joyStickPickUp; }

    public KeyCode GetKeyDash() { return keyDash; }
    public KeyCode GetJoyStickDash() { return joyStickDash; }
    public string GetHorizontalInput() { return horizontalInput; }
    public string GetVerticalInput() { return verticalInput; }
}
