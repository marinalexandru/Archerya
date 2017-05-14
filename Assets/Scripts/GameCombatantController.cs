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
        // if (AutoAttacking)
        // {
        //     return;
        // }
        // StartCoroutine(AutoAttackCoroutine());
        // AutoAttacking = true;

        timer = timer + Time.deltaTime;

        if (timer > 1 / BaseAttackSpeed)
        {
            if (CanShootTarget())
            {
                Instantiate(Projectile, Weapon.transform.position, Weapon.transform.rotation);
                ProjectileScript projectileScript = Projectile.GetComponent<ProjectileScript>();
                GameCombatantController gameCombatantController = GetTarget().gameObject.GetComponent<GameCombatantController>();
                projectileScript.ProjectileTarget = gameCombatantController.HitZone;
                Debug.DrawLine(this.HitZone.transform.position, gameCombatantController.HitZone.transform.position, Color.green,0.25f);                
                Projectile.GetComponent<ProjectileScript>().Speed = 50;
            }
            timer = 0;
        }
    }


    // private IEnumerator AutoAttackCoroutine()
    // {
    //     while (true)
    //     {
    //         if (CanShootTarget())
    //         {
    //             Instantiate(Projectile, Weapon.transform.position, Weapon.transform.rotation);
    //             ProjectileScript projectileScript = Projectile.GetComponent<ProjectileScript>();
    //             GameCombatantController gameCombatantController = GetTarget().gameObject.GetComponent<GameCombatantController>();
    //             projectileScript.ProjectileTarget = gameCombatantController.HitZone;
    //             Projectile.GetComponent<ProjectileScript>().Speed = 30;
    //         }
    //         else
    //         {
    //             StopCoroutine(AutoAttackCoroutine());
    //         }
    //         yield return new WaitForSeconds(1 / BaseAttackSpeed);
    //     }
    // }

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
