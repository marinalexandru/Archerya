using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    [HeaderAttribute("Attack:")]
    public float baseAttackSpeed = 1.0f; // attacks per second 
    public float range = 5.0f;
	[HeaderAttribute("Agro:")]
    public float agroRange = 10.0f;
    private GameObject target;
    private UnityEngine.AI.NavMeshAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        ShouldAttackTarget(); //if has target
    }


    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        if (this.target != null)
        {
            this.target.SendMessage("OnEnemyDeadOrFlee", this.GetComponent<Collider>());
        }
    }


    private void ShouldAttackTarget()
    {
        if (target == null)
        {
            return;
        }
        if (Vector3.Distance(target.transform.position, transform.position) > agroRange)
        {
			this.target.SendMessage("OnEnemyDeadOrFlee", this.GetComponent<Collider>());
            this.target = null;
			agent.SetDestination(this.transform.position);
            return;
        }

		if (Vector3.Distance(this.transform.position, target.transform.position) <= range)
        {
            // stop movement because we can attack
            agent.SetDestination(this.transform.position);
            AutoAttack();
        }
        else
        {
            // follow enemy
            agent.SetDestination(target.transform.position);
        }
    }

    private void AutoAttack()
    {
        //face the enemy
        // transform.LookAt(enemyTarget); // not interpolated -> bad

        //using quaternions
        Vector3 distanceVector = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(distanceVector);
        Quaternion stepRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                            Constants.ANGULAR_LOCK_IN_SPEED * Time.deltaTime);
        stepRotation.eulerAngles.Set(0, stepRotation.eulerAngles.y, 0);
        transform.rotation = stepRotation;

    }


    // This is called from Players that aggro 
    // this is basically the aggro function that should trigger an attack
    public void MarkTarget(GameObject target)
    {
        this.target = target;
        Debug.Log("Enemy - Will attack target");
    }

}
