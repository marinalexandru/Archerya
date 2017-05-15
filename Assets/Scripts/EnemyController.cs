using UnityEngine;
using UnityEngine.AI;

public class EnemyController : GameCombatantController
{
    public float FollowAgroRange = 20f;
    private UnityEngine.AI.NavMeshAgent Agent;
    private GameObject Target;
    // Use this for initialization
    void Start()
    {
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        ShouldFollowAgroedEnemy(); //if in range
    }


    private void ShouldFollowAgroedEnemy()
    {

        if (Target == null)
        {
            return;
        }

        if (Vector3.Distance(this.transform.position, Target.transform.position) >= FollowAgroRange)
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
    public void MarkTarget(GameObject target)
    {
        this.Target = target;
        Debug.Log("Enemy - Will attack target");
    }

    protected override GameObject GetTarget()
    {
        return Target;
    }

    protected override NavMeshAgent GetAgent()
    {
        return Agent;
    }
}
