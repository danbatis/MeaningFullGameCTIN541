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
	public float blindSearchTime=5.0f;
	public bool blindSearch = false;
	private float myTime;

	public enum hunterModes{
		patrol,
		hunt,
		fight		
	}
	public hunterModes hunterMode=hunterModes.patrol;

	public bool busy = false;

	private robotVision myRobotVisionScript;
	private BoxCollider myBoxCollider;

	// Use this for initialization
	void Start () {
		//anim = GetComponent<Animator>();
		navAgent = GetComponent<NavMeshAgent> ();
		myRobotVisionScript = GetComponent<robotVision> ();
		myBoxCollider = GetComponent<BoxCollider> ();

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
						switch (hunterMode) {
								case hunterModes.patrol:
									//patroll
									StartCoroutine (PatrolNSeek ());
									break;
								case hunterModes.hunt:
									//hunt
									if (Vector3.Distance (myTransform.position, target.position) > navAgent.stoppingDistance) {							
										if (blindSearch) {
											navAgent.SetDestination (target.position);
										} else {
											Debug.Log ("Lost subject, blindSearch started");
											blindSearch = true;
											myTime = 0.0f;
										}
									}
									else{									
										//End Game
										Debug.Log ("Found and reached player, end game!");
										Time.timeScale = 0;
										//navAgent.SetDestination (spawnPoint);
									}
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

		if (blindSearch) {
			myTime += Time.deltaTime;
			if (myTime >= blindSearchTime) {
				blindSearch = false;	
				//Lost subject
				Debug.Log ("Lost subject, returning to patrol pattern");
				hunterMode = hunterModes.patrol;
				navAgent.SetDestination (patrolPoints[currentPatrolPoint].position);
				StartCoroutine (PatrolNSeek ());
				myRobotVisionScript.enabled = false;
				myBoxCollider.enabled = true;
			}
		}
		//navAgent.pathStatus
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

	void OnTriggerEnter(Collider other){
		Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		if (other.gameObject.name == "player") {
			myRobotVisionScript.enabled = true;
			myBoxCollider.enabled = false;
		}

	}
	void OnTriggerExit(Collider other){
		Debug.Log (gameObject.name+"trigger out: "+other.gameObject.name);
		if (other.gameObject.name == "player") {
			myRobotVisionScript.enabled = false;
			myBoxCollider.enabled = true;
		}

	}
}
