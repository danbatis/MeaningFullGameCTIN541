using UnityEngine;
using System.Collections;


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
	public bool etherealForm=false;

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
	}


	//void OnControllerColliderHit(ControllerColliderHit hit){
	//	Debug.Log ("Collided with: " + hit.gameObject.name);
	//}
}
