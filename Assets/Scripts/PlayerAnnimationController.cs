using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnnimationController : MonoBehaviour
{

    private Animator playerAnimator;
	public GameObject model;

	public /// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		playerAnimator = model.GetComponent<Animator>();
	}

    public void AnimateMove()
    {
        playerAnimator.SetBool("isWalking", true);
        playerAnimator.SetBool("isAttacking", false);
    }

    public void AnimateIdle()
    {
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetBool("isAttacking", false);
    }
}
