using UnityEngine;
using System.Collections;

public class robotVision : MonoBehaviour {
	
	public float searchDistance = 5.0f;
	public float eyeHeight = 2.0f;

	public Transform hipTarget;
	public Transform headTarget;

	private robotPatrolUnit myRobotPatrolScript;
	private NavMeshAgent navAgent;


	// Use this for initialization
	void Start () {
		myRobotPatrolScript = GetComponent<robotPatrolUnit> ();
		navAgent = GetComponent<NavMeshAgent> ();

		hipTarget = GameObject.FindGameObjectWithTag("playerBody").transform;
		headTarget = GameObject.FindGameObjectWithTag ("playerHead").transform;
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;


		Debug.DrawLine(eyePosition,eyePosition+Vector3.Normalize(hipTarget.position-eyePosition)*searchDistance, new Color(0f,1f,0f));
		if (Physics.Raycast (eyePosition, hipTarget.position-eyePosition, out hit, searchDistance)) {
			//Debug.Log ("found something:" + hit.transform.name);
			if (hit.transform.tag == "Player") {
				//Debug.Log ("Found Player, updating my navmesh target and mode of operation!");
				navAgent.SetDestination (hit.transform.position);
				myRobotPatrolScript.hunterMode = robotPatrolUnit.hunterModes.hunt;
				myRobotPatrolScript.target.GetComponent<walkScript> ().SoundAlarm (gameObject.name);
				myRobotPatrolScript.busy = false;
			} else {
				Debug.DrawLine(eyePosition,eyePosition+Vector3.Normalize(headTarget.position-eyePosition)*searchDistance, new Color(1f,0f,0f));
				if (Physics.Raycast (eyePosition, headTarget.position-eyePosition, out hit, searchDistance)) {
					if (hit.transform.tag == "Player") {
						//Debug.Log ("Found Player, updating my navmesh target and mode of operation!");
						navAgent.SetDestination (hit.transform.position);
						myRobotPatrolScript.hunterMode = robotPatrolUnit.hunterModes.hunt;
						myRobotPatrolScript.target.GetComponent<walkScript> ().SoundAlarm (gameObject.name);
						myRobotPatrolScript.busy = false;
					}
				} 
			}
		}
	}
}
