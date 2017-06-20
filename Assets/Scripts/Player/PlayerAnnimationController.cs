using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnnimationController : MonoBehaviour
{

    public interface PlayerAnimationCallbacks{
        void OnProjectileShouldCreate();
        void OnProjectileShouldFire();
    }
    private Animator PlayerAnimator;
    private PlayerAnimationCallbacks AnimationCallbacks;
	public GameObject model;
    private AttackBehaviour AttackBehaviour;

	public /// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		PlayerAnimator = model.GetComponent<Animator>();
        AttackBehaviour = PlayerAnimator.GetBehaviour<AttackBehaviour>();
        AttackBehaviour.OnProjectileAttackStartedEvent += OnProjectileShouldBeInstantiated;
        AttackBehaviour.OnProjectileFireEvent += OnProjectileShouldBeFired;
	}

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        AttackBehaviour.OnProjectileAttackStartedEvent -= OnProjectileShouldBeInstantiated;
        AttackBehaviour.OnProjectileFireEvent -= OnProjectileShouldBeFired;
        AnimationCallbacks = null;
    }

    public void SetAnimationCallbacks(PlayerAnimationCallbacks PlayerAnimationCallbacks){
        this.AnimationCallbacks = PlayerAnimationCallbacks;
    }

    private void OnProjectileShouldBeInstantiated()
    {
        if (AnimationCallbacks!=null){
            AnimationCallbacks.OnProjectileShouldCreate();
        }
    }
    
    private void OnProjectileShouldBeFired()
    {
        if (AnimationCallbacks!=null){
            AnimationCallbacks.OnProjectileShouldFire();
        }
    }

    public void AnimateMove()
    {
        PlayerAnimator.SetBool("isWalking", true);
        PlayerAnimator.SetBool("isAttacking", false);
    }

    public void AnimateIdle()
    {
        PlayerAnimator.SetBool("isWalking", false);
        PlayerAnimator.SetBool("isAttacking", false);
    }

    internal void AnimateAttack()
    {
        PlayerAnimator.SetBool("isWalking", false);
        PlayerAnimator.SetBool("isAttacking", true);
    }
}
