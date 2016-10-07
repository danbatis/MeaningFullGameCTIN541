using UnityEngine;
using System.Collections;

public class villager : MonoBehaviour {
	NavMeshAgent navAgent;
	private Animator anim;   

	private Transform myTransform;

	public Vector3 spawnPoint;

	private bool busy=false;
	public Transform[] patrolPoints;
	public float patrolTime=5.0f;
	public int currentPatrolPoint=0;

	public enum villagerModes{
		patrol,
		flee,
		runHome,
	}
	public villagerModes villagerMode=villagerModes.patrol;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		navAgent = GetComponent<NavMeshAgent> ();

		myTransform = transform;
		spawnPoint = myTransform.position;
		navAgent.SetDestination (patrolPoints[currentPatrolPoint].position);

	}

	// Update is called once per frame
	void Update () {
		anim.SetFloat("samuraiForthSpeed", navAgent.velocity.magnitude/navAgent.speed); 

		if (!busy) {
			if (!navAgent.pathPending) {
				if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
					if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f) {
						// Done
						changeMode();
					}
				}
			}
		}
	}

	void changeMode(){
		switch(villagerMode){
			case villagerModes.patrol:
			StartCoroutine(PatrolNSeek ());
				break;
			case villagerModes.runHome:
				villagerMode = villagerModes.patrol;
				navAgent.SetDestination (spawnPoint);
				break;
		}
		
	}

	IEnumerator PatrolNSeek()					 {
		busy = true;
		yield return new WaitForSeconds(patrolTime);
		busy = false;
		currentPatrolPoint += 1;
		if (currentPatrolPoint >= patrolPoints.Length)
			currentPatrolPoint = 0;
		navAgent.SetDestination (patrolPoints[currentPatrolPoint].position);
	}
}
