using UnityEngine;
using System.Collections;

public class pendriveAnimator : MonoBehaviour {
	public float rotatingSpeed=50.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0f,rotatingSpeed*Time.deltaTime,0f);
	}
}
