using UnityEngine;
using System.Collections;

public class MainComputerDoor : MonoBehaviour {

	private bool MainDoorOpened;
	public bool openMainDoor;
	private float closedXcoord;
	private float currXcoord;
	public float openSpeed=5.0f;
	public float slideAmplitude=2.65f;
	private OffMeshLink dogPassage;

	void Start(){
		closedXcoord = transform.position.x;
		currXcoord = closedXcoord;

		dogPassage = GetComponent<OffMeshLink> ();
		dogPassage.activated = false;
	}

	// Update is called once per frame
	void Update () {
		if (openMainDoor && !MainDoorOpened) {
			currXcoord -= openSpeed * Time.deltaTime;
			transform.position = new Vector3(currXcoord,transform.position.y,transform.position.z);
			if (currXcoord <= closedXcoord - slideAmplitude) {
				dogPassage.activated = true;
				MainDoorOpened = true;
				openMainDoor = false;
			}
		}
	}

	void OnCollisionEnter(Collision col){
		
		if (col.gameObject.tag == "Player") {
			Debug.Log (gameObject.name+" colliding with: "+col.gameObject.name);	
		}
	}

}
