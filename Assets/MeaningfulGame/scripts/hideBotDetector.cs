using UnityEngine;
using System.Collections;

public class hideBotDetector : MonoBehaviour {

	public hideBotDoorOperator myDoorOperator;
	// Use this for initialization
	void Start () {
		myDoorOperator = GetComponentInChildren<hideBotDoorOperator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		//Debug.Log (gameObject.name+" trigger with: "+other.gameObject.name);
		if (other.gameObject.tag == "hideableBot") {			
			if(!myDoorOperator.hideDoorOpened){
				Debug.Log ("A hideable bot entered.");
				myDoorOperator.openHideDoor = true;
				myDoorOperator.closeHideDoor = false;
			}
		}

	}
	void OnTriggerExit(Collider other){
		//Debug.Log (gameObject.name+" trigger out: "+other.gameObject.name);
		if (other.gameObject.tag == "hideableBot") {
			if (myDoorOperator.hideDoorOpened) {
				Debug.Log ("A hideable bot exited.");
				myDoorOperator.closeHideDoor = true;
				myDoorOperator.openHideDoor = false;
			}
		}

	}
}
