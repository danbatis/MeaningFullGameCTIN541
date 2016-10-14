using UnityEngine;
using System.Collections;

public class easterEgg : MonoBehaviour {

	public float listenForSeconds = 5.0f;
	private float mytime;

	// Use this for initialization
	void Start () {
		mytime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.GetKey (KeyCode.RightShift) && Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.RightControl)) ||
		   (Input.GetKey (KeyCode.JoystickButton0) && Input.GetKey (KeyCode.JoystickButton1) && Input.GetKey (KeyCode.JoystickButton2) && Input.GetKey (KeyCode.JoystickButton3))) {
			Debug.Log ("easter egg...");
			Application.LoadLevel ("maze");
		}
			
		mytime += Time.time;
		if (mytime >= listenForSeconds)
			Destroy (gameObject);
	}
}
