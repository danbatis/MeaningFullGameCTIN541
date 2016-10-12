using UnityEngine;
using System.Collections;

public class autoRXwalker : MonoBehaviour {

	NavMeshAgent navAgent;
	private Animator anim;   
	public float walkAnimTreshold = 0.5f;

	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();	
		anim = GetComponent<Animator>();
		anim.SetFloat("samuraiForthSpeed", walkAnimTreshold);
	}
	
	// Update is called once per frame
	void Update () {
		if (!navAgent.pathPending) {
			if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
				if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f) {
					//arrived in front of screen, interact with it
					anim.SetBool("interactWithScreen", true);
					Debug.Log ("The RX bot should start interacting with the screen now!");
				}
			}
		}
	}
}
