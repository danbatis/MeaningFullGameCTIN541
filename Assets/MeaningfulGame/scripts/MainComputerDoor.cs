using UnityEngine;
using System.Collections;

public class MainComputerDoor : MonoBehaviour {

	private bool MainDoorOpened;
	public bool openMainDoor;
	public enum doorAxes{
		x,
		y,
		z
	} 
	public doorAxes doorAxe = doorAxes.x; 
	private float closedCoord;
	private float currCoord;
	public float openSpeed=5.0f;
	public float slideAmplitude=2.65f;
	private OffMeshLink dogPassage;

	private Transform playerTarget;
	private GameObject mainCamera;
	private AudioSource myAudio;

	void Start(){
		dogPassage = GetComponent<OffMeshLink> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		dogPassage.activated = false;

		playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
		myAudio = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		if (openMainDoor && !MainDoorOpened) {		
			if(!myAudio.isPlaying)	
				myAudio.Play ();
			currCoord -= openSpeed * Time.deltaTime;
			switch (doorAxe) {
			case doorAxes.x:
				transform.position = new Vector3(currCoord,transform.position.y,transform.position.z);
				break;
			case doorAxes.y:
				transform.position = new Vector3(transform.position.x,currCoord,transform.position.z);
				break;
			case doorAxes.z:
				transform.position = new Vector3(transform.position.x,transform.position.y,currCoord);
				break;
			}
			if (currCoord <= closedCoord - slideAmplitude) 
				DoorisOpened ();			
		}
	}

	void OnCollisionEnter(Collision col){
		
		if (col.gameObject.tag == "Player") {
			Debug.Log (gameObject.name+" colliding with: "+col.gameObject.name);	
		}
	}

	public void OpenMainDoor(){
		switch (doorAxe) {
		case doorAxes.x:
			closedCoord = transform.position.x;
			break;
		case doorAxes.y:
			closedCoord = transform.position.y;
			break;
		case doorAxes.z:
			closedCoord = transform.position.z;
			break;
		}
		currCoord = closedCoord;
		openMainDoor = true;
	}

	void DoorisOpened(){
		dogPassage.activated = true;
		MainDoorOpened = true;
		openMainDoor = false;
	}
}
