using UnityEngine;

public abstract class GameCombatantController : MonoBehaviour
{
    [HeaderAttribute("3DParts")]
    public GameObject Weapon;
    public GameObject HitZone;
    public GameObject Projectile;
    [HeaderAttribute("Attack:")]
    public float Range = 5.0f;
    public float BaseAttackSpeed = 1.0f; // attacks per second 
    private bool AutoAttacking = false;
    protected abstract GameObject GetTarget();
    protected abstract UnityEngine.AI.NavMeshAgent GetAgent();
    private float timer = 0;



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public virtual void Update()
    {
        if (GetTarget() != null)
        {
            ShouldAttackTarget(GetAgent());
        }
    }

    protected void ShouldAttackTarget(UnityEngine.AI.NavMeshAgent agent)
    {
        if (Vector3.Distance(this.transform.position, GetTarget().transform.position) <= Range)
        {
            // stop movement because we can attack
            agent.ResetPath();
            RotateTowardsTarget();
            AutoAttack();
        }
        else
        {
            // follow enemy
            agent.SetDestination(GetTarget().transform.position);
        }
    }


    private void RotateTowardsTarget()
    {
        Vector3 distanceVector = GetTarget().transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(distanceVector);
        Quaternion stepRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                            Constants.ANGULAR_LOCK_IN_SPEED * Time.deltaTime);
        stepRotation.eulerAngles.Set(0, stepRotation.eulerAngles.y, 0);
        transform.rotation = stepRotation;
    }

    private void AutoAttack()
    {
        timer = timer + Time.deltaTime;
        if (timer > 1 / BaseAttackSpeed)
        {
            if (CanShootTarget())
            {
                GameObject gameObject=Instantiate(Projectile, Weapon.transform.position, Weapon.transform.rotation);
                GameCombatantController gameCombatantController = GetTarget().gameObject.GetComponent<GameCombatantController>();
                gameObject.SendMessage("SetTarget",gameCombatantController.HitZone, SendMessageOptions.RequireReceiver);
                gameObject.SendMessage("SetSpeed",20, SendMessageOptions.RequireReceiver);
            }
            timer = 0;
        }
    }

    private bool CanShootTarget()
    {
        if (GetTarget() != null && Vector3.Distance(this.transform.position, GetTarget().transform.position) <= Range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
