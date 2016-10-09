using UnityEngine;
using System.Collections;

public class BeyondKillerArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){
		//Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		if (other.gameObject.tag == "Player") {
			other.GetComponent<walkScript> ().Die ("You jumped to the unknow, try again");
		}
	}
}
