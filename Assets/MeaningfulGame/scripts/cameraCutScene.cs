using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cameraCutScene : MonoBehaviour {

	private Transform playerTarget;
	private Transform myTransform;
	private GameObject mainCamera;

	private Camera ownCamera;
	private AudioListener ownAudioListener;
	private bool activated;
	private bool transitioning;

	public GameObject RXbot;
	public Transform MainComputerTransform;
	public Transform MainComputerScreen;
	public float smoothTrans = 3.0f;
	public float smoothTransII = 2.0f;
	public float smoothRot = 10.0f;

	public float switch2MainPcScreenTime = 3.0f;

	public float interactionDuration = 1.0f;
	public string endSceneName = "maze";

	private BoxCollider myBoxCollider;

	GameObject RXbotGO;
	Transform virtualTarget;
	public GameObject playerUI;

	private AudioSource myAudio;
	public AudioClip finalSceneEntrySound;

	// Use this for initialization
	void Start () {		
		myTransform = transform;
		//gameObject.SetActive (false);
		playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

		myBoxCollider = GetComponent<BoxCollider> ();
		ownCamera = GetComponent<Camera> ();
		ownAudioListener = GetComponent<AudioListener> ();
		ownCamera.enabled = false;
		ownAudioListener.enabled = false;

		playerUI = GameObject.Find ("Canvas");
		myAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			myTransform.LookAt (virtualTarget);
			myTransform.position = Vector3.Lerp (myTransform.position, MainComputerScreen.position, Time.deltaTime * smoothTrans);
		} 
		else {
			if (transitioning) {
				myTransform.position = Vector3.Lerp (myTransform.position, MainComputerScreen.position, Time.deltaTime * smoothTrans);
				myTransform.forward = Vector3.Lerp (myTransform.forward, MainComputerScreen.position - myTransform.position, Time.deltaTime * smoothRot);	
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "Player") {
			//get robots in pursuit
			List<string> enemiesList = other.GetComponent<walkScript>().enemiesInPursuit;
			//deactivate them
			for(int i=0; i < enemiesList.Count;i++){
				GameObject.Find (enemiesList [i]).GetComponent<robotPatrolUnit> ().returnToPatrolMode ();
			}
			//get player position and rotation
			Vector3 botOrigin = other.transform.position;
			Quaternion botRotation = other.transform.rotation;
			//deactivate player
			other.gameObject.SetActive(false);
			//add RX bot prefab
			RXbotGO = (GameObject)Instantiate(RXbot,botOrigin,botRotation);
			//set navAgent destination
			RXbotGO.GetComponent<NavMeshAgent>().SetDestination(MainComputerTransform.position);
			virtualTarget = RXbotGO.transform;
			StartCoroutine (switch2MainPcScreen());
			activated = true;
			myAudio.PlayOneShot (finalSceneEntrySound);
			mainCamera.SetActive (false);
			ownCamera.enabled = true;
			ownAudioListener.enabled = true;
			myBoxCollider.enabled = false;
			Debug.Log (gameObject.name + "trigger exit with: " + other.gameObject.name);
		}
	}

	IEnumerator switch2MainPcScreen(){
		playerUI.SetActive(false);
		yield return new WaitForSeconds(switch2MainPcScreenTime);
		activated = false;
		transitioning = true;

		smoothTrans = smoothTransII;
		virtualTarget = MainComputerScreen;

		yield return new WaitForSeconds(interactionDuration);
		Application.LoadLevel (endSceneName);
	}
}
