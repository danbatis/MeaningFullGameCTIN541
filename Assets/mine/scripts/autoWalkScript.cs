using UnityEngine;
using System.Collections;

public class autoWalkScript : MonoBehaviour {

	NavMeshAgent navAgent;
	private Animator anim;   

	public Transform target;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		navAgent = GetComponent<NavMeshAgent> ();

		if (target == null) {
			if (GameObject.FindGameObjectsWithTag ("Player")!=null)
				target = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetFloat("samuraiForthSpeed", navAgent.velocity.magnitude/navAgent.speed); 
		navAgent.SetDestination (target.position);
	}
}
