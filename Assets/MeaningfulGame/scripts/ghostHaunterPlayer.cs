using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class ghostHaunterPlayer : MonoBehaviour {

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
	public bool etherealForm=false;
	public float dashStrength=10.0f;
	public float dashMomentSpan = 0.5f;
	public bool dashing=false;
	public bool dashPitchForth=false;
	public bool dashForth=false;
	public bool dashPitchBack=false;
	public float dashPitchStrength=100f;

	// Use this for initialization
	void Start () {
		// initialising reference variables
		anim = GetComponent<Animator>();
		myControl = GetComponent<CharacterController>();
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
			if (Input.GetKeyDown ("right ctrl")) {
				Debug.Log ("player pressed right ctrl");
				StartCoroutine (DashAttack());
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

		if (Input.GetKey ("left shift")) {
			etherealForm = true;
			myTransform.gameObject.layer = 8;
		}
		else{
			etherealForm = false;
			myTransform.gameObject.layer = 10;
		}

		if (dashing) {
			if (dashPitchForth)
				myTransform.Rotate (dashPitchStrength * Time.deltaTime, 0f, 0f);
			if (dashForth)
				myControl.Move (myTransform.forward * dashStrength * Time.deltaTime);
			if (dashPitchBack)
				myTransform.Rotate (-dashPitchStrength * Time.deltaTime, 0f, 0f);
		} 
		//else {
		//	myTransform.up = Vector3.Lerp (myTransform.up, Vector3.up, Time.deltaTime * dashPitchStrength);
		//}
	}

	void OnTriggerEnter(Collider other){
		Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		if (other.gameObject.name == "samuraiCharacterRig:neck") {
			Debug.Log ("I got eyes on a ghost!");
			ghostHunter hunterScript;
			hunterScript = other.gameObject.GetComponentInParent<ghostHunter> ();
			//wake hunter
			if (hunterScript)
				hunterScript.Fight ();
			else
				Debug.Log ("Nah, it must have been just a villager...");
		}

	}
	void OnTriggerExit(Collider other){
		Debug.Log (gameObject.name+"trigger out: "+other.gameObject.name);

	}

	IEnumerator DashAttack(){
		dashing = true;
		dashPitchForth = true;
		//rotate
		yield return new WaitForSeconds(dashMomentSpan);
		dashPitchForth = false;
		dashForth = true;
		//dash
		yield return new WaitForSeconds(dashMomentSpan);
		dashForth = false;
		dashPitchBack = true;
		//rotate back
		yield return new WaitForSeconds(dashMomentSpan);
		dashPitchBack = false;
		dashing = false;

	}
}
