using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class PlayerController : GameCombatantController
{
    private UnityEngine.AI.NavMeshAgent Agent;
    private Camera Camera;
    // private List<Collider> Enemies;
    private GameObject Target;
    PlayerAnnimationController PlayerAnnimationController;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Camera = Camera.main;
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        PlayerAnnimationController = GetComponent<PlayerAnnimationController>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public override void Update()
    {
        base.Update();
        ShouldUpdateTarget(); //if clicked

        if (AgentReachedDestination())
        {
            PlayerAnnimationController.AnimateIdle();
        }
    }

    private void ShouldUpdateTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                MarkTargets(hit);
            }
        }
    }

    private void MarkTargets(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag.Equals("Enemy"))
        {
            Target = hit.transform.gameObject;
        }
        else
        {
            Target = null;
            Agent.SetDestination(hit.point);
            PlayerAnnimationController.AnimateMove();
        }

    }

    public bool AgentReachedDestination()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
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
