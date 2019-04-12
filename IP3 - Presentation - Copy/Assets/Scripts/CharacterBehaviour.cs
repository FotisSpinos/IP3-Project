using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    public abstract bool Condition(Player.Actions state);
    public abstract void ExcecuteBehaviour();
}
