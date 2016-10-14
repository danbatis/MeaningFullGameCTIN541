using UnityEngine;
using System.Collections;

public class robotVision : MonoBehaviour {
	
	public float searchDistance = 5.0f;
	public float visionAngle=15.0f;

	public Transform hipTarget;
	public Transform headTarget;

	private robotPatrolUnit myRobotPatrolScript;
	private Transform eyeTransform;
	private NavMeshAgent navAgent;


	// Use this for initialization
	void Start () {
		myRobotPatrolScript = GetComponent<robotPatrolUnit> ();
		navAgent = GetComponent<NavMeshAgent> ();

		hipTarget = GameObject.FindGameObjectWithTag("playerBody").transform;
		headTarget = GameObject.FindGameObjectWithTag ("playerHead").transform;
		eyeTransform = GameObject.Find (gameObject.name+"/Armature/Hips/Spine/Chest/Neck/Head").transform;
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;

		Debug.DrawLine(eyeTransform.position,eyeTransform.position+Vector3.Normalize(Vector3.ProjectOnPlane(eyeTransform.forward,Vector3.up)*Mathf.Cos(Mathf.Deg2Rad*visionAngle)+Vector3.ProjectOnPlane(eyeTransform.right,Vector3.up)*Mathf.Sin(Mathf.Deg2Rad*visionAngle))*searchDistance, new Color(0f,0f,1f));
		Debug.DrawLine(eyeTransform.position,eyeTransform.position+Vector3.Normalize(Vector3.ProjectOnPlane(eyeTransform.forward,Vector3.up)*Mathf.Cos(Mathf.Deg2Rad*visionAngle)-Vector3.ProjectOnPlane(eyeTransform.right,Vector3.up)*Mathf.Sin(Mathf.Deg2Rad*visionAngle))*searchDistance, new Color(0f,0f,1f));

		Debug.DrawLine(eyeTransform.position,eyeTransform.position+Vector3.Normalize(hipTarget.position-eyeTransform.position)*searchDistance, new Color(0f,1f,0f));

		if (Physics.Raycast (eyeTransform.position, hipTarget.position-eyeTransform.position, out hit, searchDistance)) {
			//Debug.Log ("found something:" + hit.transform.name);
			if (hit.transform.tag == "Player") {				
				FoundTarget (hipTarget.position, hit);
			} else {
				Debug.DrawLine(eyeTransform.position,eyeTransform.position+Vector3.Normalize(headTarget.position-eyeTransform.position)*searchDistance, new Color(1f,0f,0f));
				if (Physics.Raycast (eyeTransform.position, headTarget.position-eyeTransform.position, out hit, searchDistance)) {
					if (hit.transform.tag == "Player") {						
						FoundTarget (headTarget.position, hit);
					}
				} 
			}
		}
	}

	void FoundTarget(Vector3 targetPosition, RaycastHit hit){
		Vector3 targetDirection = targetPosition - eyeTransform.position;
		Vector3 projFloor = Vector3.ProjectOnPlane (targetDirection, eyeTransform.up);

		if (Vector3.Angle (eyeTransform.forward, projFloor) <= visionAngle) {			
			//Debug.Log (gameObject.name + " Found Player, checking angle: less than...");
			navAgent.SetDestination (targetPosition);
			myRobotPatrolScript.target = hit.transform.root;
			myRobotPatrolScript.hunterMode = robotPatrolUnit.hunterModes.hunt;
			myRobotPatrolScript.target.GetComponent<walkScript> ().SoundAlarm (gameObject.name);
			myRobotPatrolScript.busy = false;
		}
	}
}
