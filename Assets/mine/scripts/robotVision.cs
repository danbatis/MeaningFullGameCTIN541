using UnityEngine;
using System.Collections;

public class robotVision : MonoBehaviour {
	public bool foundTarget=false;
	public float searchDistance = 5.0f;
	public float eyeHeight = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		Debug.DrawLine(transform.position+Vector3.up*eyeHeight,transform.position+Vector3.up*eyeHeight+searchDistance*transform.forward, new Color(1f,0f,0f));
		if (Physics.Raycast(transform.position+Vector3.up*eyeHeight, transform.forward, out hit, searchDistance)){
			Debug.Log ("found target!"+hit.transform.name);
			//Time.timeScale = 0f;
		}
			
	
	}
}
