using UnityEngine;
using System.Collections;

public class robotPatrolUnit : MonoBehaviour {
	NavMeshAgent navAgent;
	//private Animator anim;   

	public Transform target;
	public Vector3 spawnPoint;
	public Transform[] patrolPoints;
	private Transform myTransform;
	private int currentPatrolPoint=0;

	public float patrolTime=5.0f;

	public enum hunterModes{
		patrol,
		hunt,
		fight		
	}
	public hunterModes hunterMode=hunterModes.patrol;

	public bool busy = false;

	// Use this for initialization
	void Start () {
		//anim = GetComponent<Animator>();
		navAgent = GetComponent<NavMeshAgent> ();

		if (target == null) {
			if (GameObject.FindGameObjectsWithTag ("Player")!=null)
				target = GameObject.FindGameObjectWithTag("Player").transform;
		}

		myTransform = transform;
		spawnPoint = myTransform.position;
		navAgent.SetDestination (patrolPoints[currentPatrolPoint].position);
		//navAgent.SetDestination (target.position);

	}

	// Update is called once per frame
	void Update () {
		//anim.SetFloat("samuraiForthSpeed", navAgent.velocity.magnitude/navAgent.speed);

		if (!busy) {
			if (!navAgent.pathPending) {
				if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
					if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f) {
						// Done
						Debug.Log ("arrived at destination: " + navAgent.pathStatus.ToString ());
						switch (hunterMode) {
							case hunterModes.patrol:
								//patroll
								StartCoroutine (PatrolNSeek ());
								break;
							case hunterModes.hunt:
								//hunt
								StartCoroutine (Attack());
								break;
							case hunterModes.fight:

								//navAgent.SetDestination (spawnPoint);
								break;
							default:
								Debug.Log ("Mode not implemented: " + hunterMode.ToString ());
								navAgent.SetDestination (spawnPoint);
								break;
						}
					}
				}
			}
		}
		//navAgent.pathStatus
	}

	public void Fight(){
		hunterMode = hunterModes.hunt;
		navAgent.SetDestination (target.position);
	}

	IEnumerator PatrolNSeek() {
		busy = true;
		yield return new WaitForSeconds(patrolTime);
		busy = false;
		currentPatrolPoint += 1;
		if (currentPatrolPoint >= patrolPoints.Length)
			currentPatrolPoint = 0;
		navAgent.SetDestination (patrolPoints[currentPatrolPoint].position);
	}

	IEnumerator Attack() {
		hunterMode = hunterModes.fight;
		yield return new WaitForSeconds(patrolTime/2);
		navAgent.SetDestination (target.position);
	}

	void OnTriggerEnter(Collider other){
		Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);

	}
	void OnTriggerExit(Collider other){
		Debug.Log (gameObject.name+"trigger out: "+other.gameObject.name);

	}
}
