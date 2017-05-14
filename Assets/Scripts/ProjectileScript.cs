using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    public GameObject ProjectileTarget;
    public float Speed = 0;

    // Use this for initialization
    void Start()
    {
        ProjectileTarget = null;
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
