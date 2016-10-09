using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
	
	public float smoothRot = 10.0f;		// a public variable to adjust smoothing of camera rotation
	public float smoothTrans = 3.0f;		// a public variable to adjust smoothing of camera motion

	public float rotSens = 1.0f;			// a public variable to adjust when to start rotating the camera

	private Transform myTransform;
	public Transform target;			// the usual position for the camera, specified by a transform in the game
	private Vector3 targetVirtual;
	public float camDistance = 3.0f;
	public float camHeight = 3.0f;

	private Vector3 oldTargetForth;
	private float deltaCam = 0.0f;

	void Start(){
		myTransform = transform;
		oldTargetForth = Vector3.zero;

		if (target == null) {
			Debug.Log ("Error! The targetPos was not assigned, ThirdPersonCamera script is deactivated!");
			myTransform.GetComponent<ThirdPersonCamera> ().enabled = false;
		}
	}

	void FixedUpdate ()
	{		
		float targetRot = Vector3.Angle (oldTargetForth, target.forward);

		targetVirtual = target.position - camDistance * target.forward + camHeight*target.up;
		myTransform.position = Vector3.Lerp (myTransform.position, targetVirtual, Time.deltaTime * smoothTrans);


//		if (targetRot > rotSens) {
//		Quaternion lookRotation = Quaternion.LookRotation (target.forward,target.up);
//		myTransform.rotation = lookRotation;//Quaternion.Lerp (myTransform.rotation, lookRotation, Time.deltaTime * smoothRot);
//		} 
//		else{
			myTransform.forward = Vector3.Lerp (myTransform.forward, target.forward, Time.deltaTime * smoothRot);	
		//}


		oldTargetForth = target.forward;
	}

	void OnTriggerEnter(Collider other){
		Debug.Log (gameObject.name+"trigger with: "+other.gameObject.name);
		MeshRenderer thisRenderer = other.gameObject.GetComponent<MeshRenderer> ();
		Material[] thisMaterials = thisRenderer.materials;
		for(int i=0; i< thisRenderer.materials.Length; i++){
			thisRenderer.materials[i].SetInt("_Mode",4);
			thisRenderer.materials[i].SetColor("_Color", new Color(0.5f,0.5f,1.0f,0.0f));
		}
	}
	void OnTriggerExit(Collider other){
		Debug.Log (gameObject.name+"trigger out: "+other.gameObject.name);
		MeshRenderer thisRenderer = other.gameObject.GetComponent<MeshRenderer> ();
		Material[] thisMaterials = thisRenderer.materials;
		for(int i=0; i< thisRenderer.materials.Length; i++){
			thisRenderer.materials[i].SetInt("_Mode",0);
			thisRenderer.materials[i].SetColor("_Color", new Color(1.0f,0.5f,1.0f,1.0f));
		}
	}

	void OnCollisionEnter(Collision collision){
		Debug.Log (gameObject.name+"collided with: "+collision.gameObject.name);
	}

}
