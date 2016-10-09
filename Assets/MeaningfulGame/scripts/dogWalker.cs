using UnityEngine;
using System.Collections;

public class dogWalker : MonoBehaviour {

	NavMeshAgent navAgent;
	private Transform myTransform;
	private Transform owner;
	public float happyOwnerDistance = 10.0f;

	private walkScript ownerScript;
	public Transform[] guidePoints;
	public int currentGuidePoint;

	// Use this for initialization
	void Start () {
		myTransform = transform;
		owner = GameObject.FindGameObjectWithTag ("Player").transform;
		ownerScript = owner.GetComponent<walkScript> ();

		navAgent = GetComponent<NavMeshAgent> ();
		navAgent.SetDestination (owner.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (myTransform.position, owner.position) >= happyOwnerDistance) {
			navAgent.SetDestination (owner.position);
		} 
		if (Vector3.Distance (myTransform.position, owner.position) < navAgent.stoppingDistance) {
			GuideOwner ();
		}

		if (!navAgent.pathPending) {
			if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
				if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f) {
					GuideOwner ();
				}
			}
		}
	}

	void GuideOwner(){
		if (currentGuidePoint >= guidePoints.Length) {
			navAgent.SetDestination (owner.position);
		} else {
			if (!ownerScript.GotPenDrive (guidePoints [currentGuidePoint].name)) {
				navAgent.SetDestination (guidePoints [currentGuidePoint].position);
			} 
			else {
				currentGuidePoint += 1;
				if (currentGuidePoint >= guidePoints.Length) {
					//no more guidance, just keep close to owner
					navAgent.SetDestination (owner.position);
				} 
				else {
					GuideOwner ();
				}
			}
		}
	}
}
