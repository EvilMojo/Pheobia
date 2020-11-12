using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

	public Vector2 stickInput;
	public float rotate, rotationlastFrame, modelRotationAdjustment;
	public float deadzone;
	public int rotatorLock;
	public float rotateSpeed;
	public PlayerMovement movementScript;
	public GameObject headBone, neckBone, chestBone, torsoBone;
	public float lookRotationLimit;

	// Use this for initialization
	void Start () {

		stickInput = new Vector2 (0,0);
		modelRotationAdjustment = 135.0f;
		rotationlastFrame = 0;
		deadzone = 0.25f;
		lookRotationLimit = 55.0f;
		rotateSpeed = 25.0f;
		rotatorLock = 1;
		movementScript = this.gameObject.transform.parent.GetComponent<PlayerMovement> ();

		torsoBone = this.gameObject.transform.parent.Find("playerMesh").Find("Armature").Find ("Root").Find ("Torso").gameObject;
		chestBone = torsoBone.transform.Find ("Chest").gameObject;
		neckBone =  chestBone.transform.Find("Neck").gameObject;
		headBone =  neckBone.transform.Find("Head").gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (movementScript.characterLocked == 0 && !movementScript.characterFrozen) {

			//rotate = neckBone.transform.rotation.y;
			//print (neckBone.transform.rotation

			//Rotates head manually
			//This is for manual looking around, probably not needed as navigating the player this way will be tedious and pointless
			//stickInput.y = Input.GetAxis ("PlayerRightStickY") * rotatorLock;
			//stickInput.x = Input.GetAxis ("PlayerRightStickX") * rotatorLock;
			/*stickInput.y = Input.GetAxis ("PlayerLeftStickY") * rotatorLock;
			stickInput.x = Input.GetAxis ("PlayerLeftStickX") * rotatorLock;

			if (stickInput.magnitude < deadzone) {
				stickInput = Vector2.zero;
			}



			if (stickInput.x == 0 || stickInput.y == 0) {
				rotate = rotationlastFrame;
			} else {
				if (stickInput.x >= 0) { //in bottom half
					if (stickInput.y >= 0) { //bottom right quarter
						rotate = (stickInput.x * 90);
					} else if (stickInput.y < 0) { //bottom left quarter
						rotate = -180 - (stickInput.x * 90);
					}
				} else if (stickInput.x < 0) { // in top half
					if (stickInput.y >= 0) { //top right quarter
						rotate = -90 + (stickInput.y * 90);
					} else if (stickInput.y < 0) { //top left quarter
						rotate = -90 + (stickInput.y * 90);
					}
				}
			}*/


			//Determines if the head rotation is acceptable or not
			//This is for manual looking around, probably not needed as navigating the player this way will be tedious and pointless

			/*float lookRotationDifference = neckBone.transform.eulerAngles.y - headBone.transform.eulerAngles.y;
			float lookRotationDifference2 = 360 - (neckBone.transform.eulerAngles.y - headBone.transform.eulerAngles.y);

			print (lookRotationDifference + " -=+ " + lookRotationDifference2);

			if (lookRotationDifference2 > 600) {
				print ("ERROR");
				lookRotationDifference = 0;

			} else if (lookRotationDifference > lookRotationDifference2)
				lookRotationDifference = lookRotationDifference2;

			print ("Diff: " + lookRotationDifference);

			//Keep head within range of neck
			if (lookRotationDifference < lookRotationLimit && lookRotationDifference > -lookRotationLimit) {
				//Donothing
			} else {
				print (lookRotationDifference + " BAD");
			}*/


			//WORKS
			//this.gameObject.transform.eulerAngles = new Vector3 (0, rotate+modelRotationAdjustment, 0);

			//this.gameObject.transform.rotation = Quaternion.Slerp (this.gameObject.transform.rotation, Quaternion.Euler (new Vector3 (0, -rotate + modelRotationAdjustment, 0)), rotateSpeed * Time.deltaTime);
				
			//headBone.transform.rotation = Quaternion.Euler(0,rotate+modelRotationAdjustment,-90);

			//headBone.transform.rotation = Quaternion.Slerp(headBone.transform.rotation, Quaternion.Euler(new Vector3(0, -rotate+modelRotationAdjustment, -90)), rotateSpeed * Time.deltaTime); 
			//headBone.transform.rotation = Quaternion.Slerp(headBone.transform.rotation, Quaternion.Euler(new Vector3(0, headBone.transform.parent.rotation.y, -90)), rotateSpeed * Time.deltaTime); 

			//rotationlastFrame = rotate;

		}
	}
}
