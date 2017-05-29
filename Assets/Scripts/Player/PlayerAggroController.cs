using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAggroController : MonoBehaviour
{

	public float AggroRange = 10f;
    private List<Collider> Enemies;
    private bool agroScanCoroutineStarted = false;

    // Use this for initialization
    void Start()
    {
        Enemies = new List<Collider>();
    }

    // Update is called once per frame
    void Update()
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
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, AggroRange);
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
}
