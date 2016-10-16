using UnityEngine;
using System.Collections;

public class hideBotDoorOperator : MonoBehaviour {

	public bool hideDoorOpened;

	private float closedCoord;
	private float currCoord;
	public float openSpeed=5.0f;
	public float slideAmplitude=2.65f;

	public bool openHideDoor;
	public bool closeHideDoor;

	public int openDirection=1;

	private AudioSource myAudio;

	public enum doorAxes{
		x,
		y,
		z		
	}
	public doorAxes doorAxe = doorAxes.x; 

	// Use this for initialization
	void Start () {
		myAudio = GetComponent<AudioSource> ();
		switch(doorAxe){
			case doorAxes.x:
				closedCoord = transform.position.x;
				currCoord = closedCoord;	
			break;
			case doorAxes.y:
				closedCoord = transform.position.y;
				currCoord = closedCoord;	
			break;
			case doorAxes.z:
				closedCoord = transform.position.z;
				currCoord = closedCoord;	
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (openHideDoor && !hideDoorOpened) {
			myAudio.Play ();
			switch(doorAxe){
				case doorAxes.x:
				currCoord -= openDirection*openSpeed * Time.deltaTime;
				transform.position = new Vector3(currCoord,transform.position.y,transform.position.z);
				break;
				case doorAxes.y:
				currCoord -= openDirection*openSpeed * Time.deltaTime;
				transform.position = new Vector3(transform.position.x,currCoord,transform.position.z);
				break;
				case doorAxes.z:
				currCoord -= openDirection*openSpeed * Time.deltaTime;
				transform.position = new Vector3(transform.position.x,transform.position.y,currCoord);
				break;

			}
			if (openDirection > 0) {
				if (currCoord <= closedCoord - slideAmplitude) {
					hideDoorOpened = true;
					openHideDoor = false;
				}
			} 
			else {
				if (currCoord >= closedCoord + slideAmplitude) {
					hideDoorOpened = true;
					openHideDoor = false;
				}
			}
		}
		if (closeHideDoor && hideDoorOpened) {
			myAudio.Play ();
			switch(doorAxe){
			case doorAxes.x:
				currCoord += openDirection*openSpeed * Time.deltaTime;
				transform.position = new Vector3(currCoord,transform.position.y,transform.position.z);
				break;
			case doorAxes.y:
				currCoord += openDirection*openSpeed * Time.deltaTime;
				transform.position = new Vector3(transform.position.x,currCoord,transform.position.z);
				break;
			case doorAxes.z:
				currCoord += openDirection*openSpeed * Time.deltaTime;
				transform.position = new Vector3(transform.position.x,transform.position.y,currCoord);
				break;
			}
			if (openDirection > 0) {
				if (currCoord >= closedCoord) {
					hideDoorOpened = false;
					closeHideDoor = false;
				}
			} 
			else {
				if (currCoord <= closedCoord) {
					hideDoorOpened = false;
					closeHideDoor = false;
				}
			}
		}
	}
}
