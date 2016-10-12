using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


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

	public Text playermsgText;
	public RawImage playerImage;
	public RawImage pendriveColorBlue;
	public RawImage pendriveColorPurple;
	public RawImage pendriveColorYellow;
	public RawImage pendriveColorGreen;
	public RawImage pendriveColorRed;

	private AudioSource myAudio;

	//pendrives
	public bool redPenDrive=false;
	public bool yellowPenDrive=false;
	public bool greenPenDrive=false;
	public bool bluePenDrive=false;
	public bool purplePenDrive=false;
	bool penDrivesChecked;
	public bool redPenDriveQuestion=false;
	private GameObject redPenDriveGO;

	//redpendrive alarm
	private bool RPalarm;
	private float redpendriveInitTime;
	public int redpendriveSpan = 60;
	private int redpendriveAlarmCounter;
	public Text redpendriveAlarmCounterUI;
	private GameObject[] enemies;
	public AudioClip RPalarmSound;

	private bool dead;
	private bool pausedGame;

	public float TimeToEraseMsg=2.0f;
	public GameObject torchLight;

	public AudioClip alarmSound;
	public AudioClip defaultSound;

	public List<string> enemiesInPursuit;


	// Use this for initialization
	void Start () {
		// initialising reference variables
		anim = GetComponent<Animator>();
		myControl = GetComponent<CharacterController>();
		myAudio = GetComponent<AudioSource> ();


		if (!torchLight)
			Debug.Log ("The torch light element must be assigned!");
		
		playermsgText.text = "";
		//playerImage.enabled = false;
		playerImage.CrossFadeAlpha(0f,0f,true);

		myTransform = transform;
		redpendriveAlarmCounterUI.enabled = false;
		enemies = GameObject.FindGameObjectsWithTag ("hideableBot");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("escape")){
			if (pausedGame) {
				pausedGame = false;
				Time.timeScale = 1;
			} else {
				pausedGame = true;
				Time.timeScale = 0;
			}

		}

		if (dead) {
			playermsgText.enabled = true;
			playerImage.CrossFadeAlpha (1.0f,1.0f,true);
			StartCoroutine (RestartLevel(1.5f));
			}
		else{
			if(RPalarm){
				if (Input.GetKeyDown ("right ctrl")) {
					Debug.Log ("pressed right ctrl");
					if (redPenDrive) {
						redPenDrive = false;
						StartCoroutine(activateRPafter(TimeToEraseMsg));
						redPenDriveGO.transform.position = myTransform.position+0.7f*Vector3.up;
					}
				} 
				else {
					//emits signal to draw all robots from time to time
					if (Time.time - redpendriveInitTime >= 1) {
						redpendriveAlarmCounter -= 1;
						if (redpendriveAlarmCounter < 0) {
							redpendriveAlarmCounter = redpendriveSpan;
							if (redPenDrive) {
								emitSignal2AttractRobots (myTransform.position);
							} 
							else {
								emitSignal2AttractRobots (redPenDriveGO.transform.position);
							}
						}
						redpendriveAlarmCounterUI.text = redpendriveAlarmCounter.ToString ();
						redpendriveInitTime = Time.time;
					}
				}
			}
			if (redPenDriveQuestion) {			
				if (Input.GetKeyDown ("y")) {
					playermsgText.text = "red pen drive acquired, you better run, good luck";
					redPenDrive = true;
					redPenDriveGO.SetActive (false);
					Time.timeScale = 1;
					redPenDriveQuestion = false;
					initializeRPalarm ();
					StartCoroutine(changePlayerMsg(""));
				}
				if (Input.GetKeyDown ("n")) {
					playermsgText.text = "red pen drive discarded, you will need it later";
					redPenDriveGO.SetActive (false);

					Time.timeScale = 1;
					redPenDriveQuestion = false;
					StartCoroutine(changePlayerMsg(""));
					StartCoroutine(activateRPafter(TimeToEraseMsg));
				}

			}
			if(Input.GetKeyDown("left shift"))
				myTransform.Rotate(0f,180f,0f);

			if (yellowPenDrive) {
				torchLight.SetActive (true);
			} else {
				torchLight.SetActive (false);
			}
				
			// setup h variable as our horizontal input axis
			float h = Input.GetAxis("Horizontal");              

			myTransform.Rotate(0f,h*rotateSpeed*Time.deltaTime,0f);

			if (inAir) {
				if ((myControl.collisionFlags & CollisionFlags.Below) != 0) {
					inAir = false;
					anim.SetBool ("JumpDown", true);
					anim.SetBool ("JumpUp", false);
				}
			} else {
				vertIN = Input.GetAxis("Vertical");
				anim.SetFloat("samuraiForthSpeed", vertIN);
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
					vertIN = 0f;
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
	}


	void OnControllerColliderHit(ControllerColliderHit hit){
		if (hit.gameObject.name == "mainRoomDoor") {
			if (!penDrivesChecked) {				
				//CheckPenDrives
				string msg;
				if (redPenDrive && yellowPenDrive && greenPenDrive && purplePenDrive && bluePenDrive) {
					if (enemiesInPursuit.Count > 0) {
						msg = "Come back without robots in pursuit";
					} 
					else {
						msg = "Ok, You have all of the PenDrives!";
						hit.gameObject.GetComponent<MainComputerDoor> ().openMainDoor = true;
					}
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

			pendriveColorBlue.enabled = bluePenDrive;
			pendriveColorPurple.enabled = purplePenDrive;
			pendriveColorGreen.enabled = greenPenDrive;
			pendriveColorYellow.enabled = yellowPenDrive;
			pendriveColorRed.enabled = redPenDrive;
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
			//Debug.Log (gameObject.name+"check pendrives: "+other.gameObject.name);
			StartCoroutine (changePlayerMsg (""));
		}

	}

	bool PickPenDrive(GameObject testGO, string pendriveName, bool whichPenDrive){
		if (testGO.name == pendriveName) {
			if (pendriveName == "redPenDrive") {
				playermsgText.text = "The red pendrive emits a signal attracting robots every "+redpendriveSpan.ToString()+" seconds";
				StartCoroutine(changePlayerMsg("You sure you want to take the redPenDrive? (y) or (n)"));
				redPenDriveQuestion = true;
				redPenDriveGO = testGO;
				Time.timeScale = 0;
				return false;
			} else {
				if (!whichPenDrive) {
					string msg = "Congratulations, you acquired the " + pendriveName + " pen drive.";
					Debug.Log (msg);
					playermsgText.text = msg;
					StartCoroutine(changePlayerMsg(""));
					testGO.SetActive (false);
					return(true);
				} else {
					return(whichPenDrive);
				}
			}
		} else { return(whichPenDrive);
		}
	}
	IEnumerator changePlayerMsg(string msg){
		yield return new WaitForSecondsRealtime(TimeToEraseMsg);
		playermsgText.text = msg;
	}
	IEnumerator activateRPafter(float timeSpan){
		yield return new WaitForSeconds(timeSpan);
		redPenDriveGO.SetActive (true);
	}

	public void Die(string msg){
		if (!dead) {
			dead = true;
			playermsgText.text = msg;
			playermsgText.enabled = false;
		}
	}
	IEnumerator RestartLevel(float timespan){
		yield return new WaitForSecondsRealtime(timespan);
		Application.LoadLevel (Application.loadedLevel);
		Time.timeScale = 1;
	}

	public bool GotPenDrive(string pendriveName){
		switch (pendriveName) {
		case "yellowPenDrive":
			return yellowPenDrive;
			break;
		case "redPenDrive":
			return redPenDrive;
			break;
		case "greenPenDrive":
			return greenPenDrive;
			break;
		case "purplePenDrive":
			return purplePenDrive;
			break;
		case "bluePenDrive":
			return bluePenDrive;
			break;
		default:
			return false;
			break;
		}
	}

	public void SoundAlarm(string enemyName){
		if(!enemiesInPursuit.Contains(enemyName)){
			enemiesInPursuit.Add (enemyName);

			myAudio.clip = alarmSound;
			myAudio.Play ();

			//Debug.Log("current audio clip is "+myAudio.clip.ToString());
		}
	}
	public void StopSoundAlarm(string enemyName){
		enemiesInPursuit.Remove (enemyName);
		if (enemiesInPursuit.Count == 0) {			
			myAudio.clip = defaultSound;
			myAudio.Play ();
			Debug.Log ("current audio clip is " + myAudio.clip.ToString ());
		}
	}

	void emitSignal2AttractRobots(Vector3 attractTo){
		myAudio.PlayOneShot (RPalarmSound);
		for (int i = 0; i < enemies.Length; i++) {
			enemies [i].GetComponent<NavMeshAgent> ().SetDestination (attractTo);
			enemies [i].GetComponent<robotPatrolUnit> ().hunterMode = robotPatrolUnit.hunterModes.hunt;
			enemies [i].GetComponent<robotPatrolUnit> ().busy = false;
			SoundAlarm (enemies[i].name);			
		}
	}
	void initializeRPalarm(){
		redpendriveInitTime = Time.time;
		redpendriveAlarmCounterUI.enabled = true;
		redpendriveAlarmCounter = redpendriveSpan;
		redpendriveAlarmCounterUI.text = redpendriveAlarmCounter.ToString ();
		RPalarm = true;
		emitSignal2AttractRobots (myTransform.position);
	}
}
