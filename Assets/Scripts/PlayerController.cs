using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class PlayerController : GameCombatantController
{
    private UnityEngine.AI.NavMeshAgent Agent;
    private Camera Camera;
    private GameObject FriendlyTarget;
    private List<Collider> Enemies;
    private GameObject Target;
    private bool agroScanCoroutineStarted = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Camera = Camera.main;
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Enemies = new List<Collider>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    public override void Update()
    {
        base.Update();
        ShouldUpdateTarget(); //if clicked
        ShouldTalkToFriend(); //if friend targeted
        ShouldGetAggroOfEnemy(); //if enemys are near 
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
        switch (hit.transform.gameObject.tag)
        {
            case "Enemy":
                Target = hit.transform.gameObject;
                FriendlyTarget = null;
                break;
            case "Friend":
                FriendlyTarget = hit.transform.gameObject;
                Target = null;
                break;
            default:
                Target = null;
                FriendlyTarget = null;
                Agent.SetDestination(hit.point);
                break;
        }
    }

    private void ShouldTalkToFriend()
    {

    }

    private void ShouldGetAggroOfEnemy()
    {
        if (!agroScanCoroutineStarted)
        {
            StartCoroutine(AggroScanCoroutine());
            agroScanCoroutineStarted = true;
        }
    }


    // this is called by an attacking enemy on demise or on flee
    public void OnEnemyDeadOrFlee(Collider collider)
    {
        Enemies.Remove(collider);
        Debug.Log("Player - Enemy removed from list. Size is: " + Enemies.Count);
    }

    private IEnumerator AggroScanCoroutine()
    {
        //todo try InvokeRepeating also
        while (true)
        {

            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, Range);

            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag.Equals("Enemy"))
                {
                    //already send message to ennemy no need to spam him :) 
                    if (!Enemies.Contains(hitColliders[i]))
                    {
                        Enemies.Add(hitColliders[i]);
                        hitColliders[i].gameObject.SendMessage("MarkTarget", this.gameObject);
                    }
                }
                i++;
            }
            yield return new WaitForSeconds(Constants.CAST_AGGRO_FREQUENCY);

        }
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
