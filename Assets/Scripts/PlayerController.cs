using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [HeaderAttribute("Attack:")]
    public float baseAttackSpeed = 1.0f; // attacks per second 
    public float range = 5.0f;
    public GameObject projectile;
    private UnityEngine.AI.NavMeshAgent agent;
    private Camera camera;
    private Transform friendlyTarget;
    private Transform enemyTarget;
    private List<Collider> enemies;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        camera = Camera.main;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        enemies = new List<Collider>();
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        ShouldUpdateTarget(); //if clicked
        ShouldAttackEnemy(); //if enemy targeted
        ShouldTalkToFriend(); //if friend targeted
        ShouldGetAggroOfEnemy(); //if enemys are near 
    }

    private void ShouldUpdateTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                MarkTargets(hit);
            }
        }
    }

    private void MarkTargets(RaycastHit hit)
    {
        switch (hit.transform.gameObject.tag)
        {
            case "Enemy":
                enemyTarget = hit.transform;
                friendlyTarget = null;
                break;
            case "Friend":
                friendlyTarget = hit.transform;
                enemyTarget = null;
                break;
            default:
                enemyTarget = null;
                friendlyTarget = null;
                agent.SetDestination(hit.point);
                break;
        }
    }

    private void ShouldTalkToFriend()
    {

    }
    private void ShouldAttackEnemy()
    {
        if (enemyTarget == null)
        {
            return;
        }
        if (Vector3.Distance(this.transform.position, enemyTarget.transform.position) <= range)
        {
            // stop movement because we can attack
            agent.SetDestination(this.transform.position);
            AutoAttack();
        }
        else
        {
            // follow enemy
            agent.SetDestination(enemyTarget.transform.position);
        }
    }

    private void AutoAttack()
    {
        //face the enemy
        // transform.LookAt(enemyTarget); // not interpolated -> bad

        //using quaternions
        Vector3 distanceVector = enemyTarget.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(distanceVector);
        Quaternion stepRotation = Quaternion.Slerp(transform.rotation, targetRotation,
                            Constants.ANGULAR_LOCK_IN_SPEED * Time.deltaTime);
        stepRotation.eulerAngles.Set(0, stepRotation.eulerAngles.y, 0);
        transform.rotation = stepRotation;

        FireProjectile();
    }

    private bool started = false;

    private void FireProjectile()
    {
        if (started)
        {
            return;
        }
        StartCoroutine(FireProjectileCoroutine());
        started = true;
    }

    private void ShouldGetAggroOfEnemy()
    {
        StartCoroutine(AggroScanCoroutine());
    }

    private IEnumerator FireProjectileCoroutine()
    {
        while (true)
        {
            if (enemyTarget != null)
            {
                Instantiate(projectile, this.transform.position, transform.rotation);
                projectile.GetComponent<ProjectileScript>().ProjectileTarget = enemyTarget;
                projectile.GetComponent<ProjectileScript>().Speed = 30;
            }
            yield return new WaitForSeconds(1/baseAttackSpeed);
        }
    }

    // this is called by an attacking enemy on demise or on flee
    public void OnEnemyDeadOrFlee(Collider collider)
    {
        enemies.Remove(collider);
        Debug.Log("Player - Enemy removed from list. Size is: " + enemies.Count);
    }

    private IEnumerator AggroScanCoroutine()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, range);

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag.Equals("Enemy"))
            {
                //already send message to ennemy no need to spam him :) 
                if (!enemies.Contains(hitColliders[i]))
                {
                    Debug.Log("Player - Enemy added to list");
                    enemies.Add(hitColliders[i]);
                    hitColliders[i].gameObject.SendMessage("MarkTarget", this.gameObject);
                }
            }
            i++;
        }
        yield return new WaitForSeconds(Constants.CAST_AGGRO_FREQUENCY);
    }

}
