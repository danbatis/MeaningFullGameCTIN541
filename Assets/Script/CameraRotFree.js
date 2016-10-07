#pragma strict
var selectTargetObject:Transform;


 
function Update () {
		this.transform.LookAt (selectTargetObject);
		  this.transform.RotateAround (selectTargetObject.position, Vector3.up, 30 * Time.deltaTime);
 
}
