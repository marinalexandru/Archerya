using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float FollowAgroRange = 20f;
    private UnityEngine.AI.NavMeshAgent Agent;
    private GameObject Target;
    private ShooterController ShooterController;

    // Use this for initialization
    void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        ShooterController = GetComponent<ShooterController>();
    }

    // Update is called once per frame
    public void Update()
    {
        ShooterController.SetTarget(Target);
        if (Target != null && Vector3.Distance(this.transform.position, Target.transform.position) >= FollowAgroRange)
        {
            Agent.ResetPath();
            this.Target.SendMessage("OnEnemyDeadOrFlee", this.GetComponent<Collider>());
            Target = null;
        }
    }


    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (this.Target != null)
        {
            this.Target.SendMessage("OnEnemyDeadOrFlee", this.GetComponent<Collider>());
        }
    }

    // This is called from Players that aggro 
    // this is basically the aggro function that should trigger an attack
    public void FollowTarget(GameObject target)
    {
        this.Target = target;
        Debug.Log("Enemy - Will attack target");
    }

}
