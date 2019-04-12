using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    private Animator animator;
    private string[] animations;
    Player player;

    public void Start()
    {
        GameObject parent = transform.parent.gameObject;
        if (parent != null)
            player = parent.GetComponent<Player>();
        if (GetComponent<Animator>())
            animator = GetComponent<Animator>();

        animations = new string[4];
        animations[0] = "Attack";
        animations[1] = "Walk";
        animations[2] = "Dash";
        animations[3] = "Idle";
    }

    public void UpdateAnimation(Player.Actions currentState)
    {
        switch (currentState)
        {
            case Player.Actions.IDLE:
                ActivateAnimation("Idle");
                break;

            case Player.Actions.MOVETOSIDE:
                ActivateAnimation("Walk");
                break;

            case Player.Actions.HOLDBUILDING:
                StartCoroutine(ApplyWalkOrRun(player.GetState()));
                break;

            case Player.Actions.WALK:
                ActivateAnimation("Walk");
                break;

            case Player.Actions.DASH:
                ActivateAnimation("Dash");
                break;

            case Player.Actions.ATTACK:
                ActivateAnimation("Attack");
                break;
        }
    }

    private IEnumerator ApplyWalkOrRun(Player.Actions state)
    {
        Quaternion playerRotation = transform.rotation;

        while(player.GetState() == state)
        {
            if (transform.parent.GetComponent<CharacterController>().velocity != Vector3.zero || transform.rotation != playerRotation)           
                ActivateAnimation("Walk");           
            else
                ActivateAnimation("Idle");

            playerRotation = transform.rotation;

            yield return null;
        }
    }
    
    public void ActivateAnimation(string animName)
    {
        for(int i = 0; i < animations.Length; i++)
        {
            if (animName == animations[i])
                animator.SetBool(animations[i], true);            
            else
                animator.SetBool(animations[i], false);
        }
    }
}
