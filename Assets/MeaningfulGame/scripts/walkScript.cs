using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(CharacterController))]

public class walkScript : MonoBehaviour {

	public float rotateSpeed=10.0f;
	public float forwardSpeed=10.0f;

	private Animator anim;   
	public float animSpeed = 1.5f;              // a public setting for overall animator animation speed

	private CharacterController myControl;                    // a reference to the capsule collider of the character
	private Transform myTransform;

	public float verticalSpeed;
	public float verticalSpeedMax = 5.0f;
	public float verticalAcel = 10.0f;

	public bool inAir=false;
	private float vertIN = 0f;                // setup v variables as our vertical input axis

	public Canvas myCanvas;
	public Text playermsgText;

	//pendrives
	public bool redPenDrive=false;
	public bool yellowPenDrive=false;
	public bool greenPenDrive=false;
	public bool bluePenDrive=false;
	public bool purplePenDrive=false;
	bool penDrivesChecked;


	// Use this for initialization
	void Start () {
		// initialising reference variables
		anim = GetComponent<Animator>();
		myControl = GetComponent<CharacterController>();
		//playermsgText = myCanvas.GetComponent<Text> ();
		playermsgText.text = "";
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis("Horizontal");              // setup h variable as our horizontal input axis


		myTransform.Rotate(0f,h*rotateSpeed*Time.deltaTime,0f);

		if (inAir) {
			if ((myControl.collisionFlags & CollisionFlags.Below) != 0) {
				inAir = false;
				anim.SetBool ("JumpDown", true);
				anim.SetBool ("JumpUp", false);
			}
		} else {
			vertIN = Input.GetAxis("Vertical");
			anim.SetFloat("samuraiForthSpeed", vertIN);                          // set our animator's float parameter 'Speed' equal to the vertical input axis				
		}
		if (myControl.isGrounded) {
			if (Input.GetKeyDown ("space")) {
				verticalSpeed = verticalSpeedMax;
				inAir = true;
				anim.SetFloat("samuraiForthSpeed", 0f);
				anim.SetBool ("JumpUp",true);
				anim.SetBool ("JumpDown",false);
			}
			if (Input.GetKey (KeyCode.LeftControl)) {
				anim.SetBool ("crouch", true);
			} else {
				anim.SetBool ("crouch", false);
			}
		}
		else{
			verticalSpeed -= verticalAcel * Time.deltaTime;
			if (verticalSpeed <= -verticalSpeedMax)
				verticalSpeed = -verticalSpeedMax;
		}

		float forthSpeed;
		if (vertIN > 0)
			forthSpeed = forwardSpeed;
		else
			forthSpeed = forwardSpeed/2;
		myControl.Move(myTransform.forward * vertIN * forthSpeed*Time.deltaTime + myTransform.up*verticalSpeed*Time.deltaTime );

		anim.speed = animSpeed;
		//Debug.Log ("grounded: "+myControl.isGrounded.ToString());
	}


	void OnControllerColliderHit(ControllerColliderHit hit){
		/*
		if (hit.gameObject.name == "yellowPenDrive") {
			if (!yellowPenDrive) {
				Debug.Log ("Congratulations, you acquired the yellow pen drive.");
				yellowPenDrive = true;
				hit.gameObject.SetActive (false);
			}
		}


		if (hit.gameObject.name == "greenPenDrive") {
			if (!greenPenDrive) {
				Debug.Log ("Congratulations, you acquired the green pen drive.");
				greenPenDrive = true;
				hit.gameObject.SetActive (false);
			}
		}
		if (hit.gameObject.name == "purplePenDrive") {
			if (!purplePenDrive) {
				Debug.Log ("Congratulations, you acquired the purple pen drive.");
				purplePenDrive = true;
				hit.gameObject.SetActive (false);
			}
		}
		if (hit.gameObject.name == "bluePenDrive") {
			if (!bluePenDrive) {
				Debug.Log ("Congratulations, you acquired the blue pen drive.");
				bluePenDrive = true;
				hit.gameObject.SetActive (false);
			}
		}
		*/
		if (hit.gameObject.name == "mainRoomDoor") {
			if (!penDrivesChecked) {				
				//CheckPenDrives
				string msg;
				if (redPenDrive && yellowPenDrive && greenPenDrive && purplePenDrive && bluePenDrive) {
					msg = "Ok, You have all of the PenDrives!";
					hit.gameObject.GetComponent<MainComputerDoor> ().openMainDoor = true;
				} else {
					msg = "You do not have all of the PenDrives! Come Back when you do.";
				}
				Debug.Log (msg);
				playermsgText.text = msg;
				penDrivesChecked = true;
			}
		}
		else{
			yellowPenDrive=PickPenDrive (hit.gameObject, "yellowPenDrive", yellowPenDrive);
			redPenDrive=PickPenDrive (hit.gameObject, "redPenDrive", redPenDrive);
			greenPenDrive=PickPenDrive (hit.gameObject, "greenPenDrive", greenPenDrive);
			bluePenDrive=PickPenDrive (hit.gameObject, "bluePenDrive", bluePenDrive);
			purplePenDrive=PickPenDrive (hit.gameObject, "purplePenDrive", purplePenDrive);
		}
	}
	/*
	void OnTriggerEnter(Collider other){
		//Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		if (other.gameObject.name == "mainDoorRegion") {
			
			Debug.Log (gameObject.name+"check pendrives: "+other.gameObject.name);
		}

	}
	*/
	void OnTriggerExit(Collider other){
		//Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		if (other.gameObject.name == "mainDoorRegion") {
			penDrivesChecked = false;
			Debug.Log (gameObject.name+"check pendrives: "+other.gameObject.name);
			playermsgText.text = "";
		}

	}

	bool PickPenDrive(GameObject testGO, string pendriveName, bool whichPenDrive){
		if (testGO.name == pendriveName) {
			if (!whichPenDrive) {
				Debug.Log ("Congratulations, you acquired the " + pendriveName + " pen drive.");
				testGO.SetActive (false);
				return(true);
			} else {
				return(whichPenDrive);
			}
		} else { return(whichPenDrive);
		}
	}
}
