using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	
	private List<Collider> Enemies;
	private PlayerAggroController PlayerAggroController;

	// Use this for initialization
	void Start () {
		PlayerAggroController = GetComponent<PlayerAggroController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
