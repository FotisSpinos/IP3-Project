using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : CharacterBehaviour
{
    [SerializeField] private float destroyRange;
    private TargetFunctions tf;
    private bool canAttack;
    private bool attacking;

    [SerializeField] private float animationTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float validAngle;

    private Controls controls;
    [SerializeField] public byte damage;

    public byte GetDamage() { return damage; }

    private void Start()
    {
        if (!GetComponent<TargetFunctions>())        
            gameObject.AddComponent<TargetFunctions>();
        
        tf = GetComponent<TargetFunctions>();
        tf.Initialize();

        canAttack = true;
        attacking = false;

        if (!GetComponent<Player>())
            gameObject.AddComponent<Controls>();

        controls = GetComponent<Player>().GetControls();
    }

    private IEnumerator AttackAnimation(GameObject selectedBuilding)
    {
        canAttack = false;
        attacking = true;

        yield return new WaitForSeconds(animationTime); //Animation time 0.73f

        if (selectedBuilding != null && selectedBuilding.tag == "Building")
        {
            selectedBuilding.GetComponent<BuildingStats>().Damage(damage, transform);
        }
        attacking = false;

        // Can Attack Again
        yield return new WaitForSeconds(attackCooldown);  //1.0
        canAttack = true;
    }

    public override bool Condition(Player.Actions state)
    {        
        if (state == Player.Actions.HOLDBUILDING ||
            state == Player.Actions.MOVETOSIDE ||
            state == Player.Actions.REACHEDTOTARGET ||
            state == Player.Actions.DASH)
            return false;

        if (attacking)
            return true;

        if ((Input.GetKeyDown(controls.GetAttackKey()) || Input.GetKeyDown(controls.GetAttackJoyStick())))
            if(canAttack)
                return true;        
        return false;
    }

    public override void ExcecuteBehaviour()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, destroyRange);
        if (colliders.Length == 0)       
            return;
        
        TargetFunctions bl = GetComponent<TargetFunctions>();
        PosFunctions ps = GetComponent<PosFunctions>();

        List<Collider> buildings = bl.FindTargetsInRange(colliders);

        if (buildings != null && buildings.Count > 0)
        {
            GameObject selectedBuilding = bl.SelectBuildingToPick(validAngle);

            if (selectedBuilding != null && !attacking)
            {
                StartCoroutine(AttackAnimation(selectedBuilding));
            }
        }
        else
            StartCoroutine(AttackAnimation(null));
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, destroyRange);
    }
}
