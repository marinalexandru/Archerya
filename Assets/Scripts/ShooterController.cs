using UnityEngine;
using UnityEngine.AI;

public class ShooterController : MonoBehaviour
{
    [HeaderAttribute("3DParts")]
    public GameObject Weapon;
    public GameObject HitZone;
    public GameObject Projectile;
    [HeaderAttribute("Attack:")]
    public float Range = 5.0f;
    public float BaseAttackSpeed = 1.0f; // attacks per second 
    private bool AutoAttacking = false;
    private float timer = 0;
    private NavMeshAgent Agent;
    private GameObject Target;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public void Update()
    {
        if (Target != null)
        {
            if (Vector3.Distance(this.transform.position, Target.transform.position) <= Range)
            {
                // stop movement because we can attack
                Agent.ResetPath();
                RotateTowardsTarget();
                AutoAttack();
            }
            else
            {
                // follow enemy
                Agent.SetDestination(Target.transform.position);
            }
        }
    }

    public void SetTarget(GameObject Target)
    {
        this.Target = Target;
    }

    private void RotateTowardsTarget()
    {
        Vector3 distanceVector = Target.transform.position - transform.position;
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
                GameObject gameObject = Instantiate(Projectile, Weapon.transform.position, Weapon.transform.rotation);
                ShooterController gameCombatantController = Target.gameObject.GetComponent<ShooterController>();
                gameObject.SendMessage("SetTarget", gameCombatantController.HitZone, SendMessageOptions.RequireReceiver);
                gameObject.SendMessage("SetSpeed", 20, SendMessageOptions.RequireReceiver);
            }
            timer = 0;
        }
    }

    private bool CanShootTarget()
    {
        if (Target != null && Vector3.Distance(this.transform.position, Target.transform.position) <= Range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
