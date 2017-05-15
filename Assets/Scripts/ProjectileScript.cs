using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    private GameObject ProjectileTarget;
    private float Speed = 0;

    public void SetTarget(GameObject ProjectileTarget){
        this.ProjectileTarget = ProjectileTarget;
    }

    public void SetSpeed(float Speed){
        this.Speed = Speed;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.ProjectileTarget != null)
        {
            float step = Speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, ProjectileTarget.transform.position, step);
        }
        else
        {
            Destroy(gameObject, 0);
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Destroy(gameObject, 2);
    }

}
