using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public Vector2 stickInput;
	public float rotatorLock;
	public float moveSpeed;	
	public float rotate, rotationlastFrame, modelRotationAdjustment;
	public float rotateSpeed;
	public Rigidbody rb;
	public float deadzone;
	public int characterLocked; //Frozen for only a short while
	public bool characterFrozen; //Frozen for an indefinite amount of time
	public string facingLock; //Only used when movement is locked to an axis, to let us go backward
	public enum shimmyLock{
		unlocked,
		xlockedFacingNorth,
		xlockedFacingSouth,
		zlockedFacingNorth,
		zlockedFacingSouth,
	}//0 Not locked - 1 Locked to x - 2 locked to z
	public shimmyLock shimmy;
	public GameObject leftShimmyReach, rightShimmyReach;


	// Use this for initialization
	void Start () {
		leftShimmyReach = new GameObject ();
		leftShimmyReach.transform.parent = this.gameObject.transform;
		rightShimmyReach = new GameObject ();
		rightShimmyReach.transform.parent = this.gameObject.transform;

		leftShimmyReach.transform.position = new Vector3 (this.gameObject.transform.position.x - this.gameObject.GetComponent<Collider> ().bounds.size.x / 2, 
			this.gameObject.transform.position.y + this.gameObject.GetComponent<Collider>().bounds.size.y + 2, 
			this.gameObject.transform.position.z + this.gameObject.GetComponent<Collider>().bounds.size.z);
		
		rightShimmyReach.transform.position = new Vector3 (this.gameObject.transform.position.x + this.gameObject.GetComponent<Collider> ().bounds.size.x / 2, 
			this.gameObject.transform.position.y + this.gameObject.GetComponent<Collider>().bounds.size.y + 2, 
			this.gameObject.transform.position.z + this.gameObject.GetComponent<Collider>().bounds.size.z);

		shimmy = shimmyLock.unlocked;
		stickInput = new Vector2(0,0);
		facingLock = "None";
		modelRotationAdjustment = 135.0f;
		deadzone = 0.25f;
		moveSpeed = 3.0f;
		rotatorLock = 1.0f;
		rotateSpeed = 25.0f;
		rotationlastFrame = 0;
		rb = this.gameObject.transform.gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);*/

		//Things to change

		//Player to rotate to direction of walking
		//Player to be looking at direction of empty object (LookTarget)
		/*if (!checkFloor ()) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).gameObject.name != "LookCentre" &&
				    transform.GetChild (i).gameObject.name != "playerMesh" &&
				    transform.GetChild (i).gameObject.name != "PlayerSpawnPoint" &&
				    transform.GetChild (i).gameObject.name != "bodyMesh") {
					GameObject dropThis = transform.GetChild (i).gameObject;
					if (dropThis.GetComponent<DetectFloor> () != null) {
						dropThis.GetComponent<DetectFloor> ().release ();
					}
					SceneManager.MoveGameObjectToScene (dropThis, SceneManager.GetActiveScene());

				}
			}
		}*/

		if (characterLocked <= 0 && !characterFrozen) {
			float moveZ = -Input.GetAxis ("PlayerLeftStickY");
			float moveX = Input.GetAxis ("PlayerLeftStickX");

			stickInput.y = Input.GetAxisRaw ("PlayerLeftStickY");
			stickInput.x = Input.GetAxisRaw ("PlayerLeftStickX") * -1;

			//print (stickInput);


			rotateSpeed = 25.0f;

			if (stickInput.magnitude < deadzone) {
				stickInput = Vector2.zero;
				moveSpeed = 0.0f;
				rotateSpeed = 0.0f;
				rotate = rotationlastFrame;
			} else {
				rotate = Mathf.Atan2 (stickInput.x, stickInput.y) * Mathf.Rad2Deg;
				rotationlastFrame = rotate;
			}
				
			//print ("LStick: " + stickInput + " = " + rotate + " degrees");

			//this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, Quaternion.LookRotation(new Vector3(-stickInput.x, 0, stickInput.y)), Time.deltaTime*8);
			//this.gameObject.transform.eulerAngles = new Vector3 (0, -rotate+modelRotationAdjustment, 0);

			//Parent ROTATION
			if (shimmy == shimmyLock.unlocked) {
				this.gameObject.transform.rotation = Quaternion.Slerp (this.gameObject.transform.rotation, Quaternion.Euler (new Vector3 (0, rotate + modelRotationAdjustment, 0)), rotateSpeed * Time.deltaTime * rotatorLock);
			}
			//Self ROTATION (if script attached to child)
			//this.gameObject.transform.parent.rotation = Quaternion.Slerp(this.gameObject.transform.parent.rotation, Quaternion.Euler(new Vector3(0,-rotate+modelRotationAdjustment,0)), rotateSpeed * Time.deltaTime);

			//print (Quaternion.LookRotation(new Vector3(-stickInput.x, 0, stickInput.y)));

			//Parent MOVEMENT
			//this.gameObject.transform.parent.Translate (moveX * Time.deltaTime * moveSpeed * moveXMultiplier, 0, moveZ * Time.deltaTime * moveSpeed * moveZMultiplier);
			int moveBackInt = 0;
			if (facingLock == "None") {
				moveBackInt = 1;
				if (shimmy != shimmyLock.unlocked) {
					if (stickInput.x < 0)
						moveBackInt = -1;
					else
						moveBackInt = 1;
				}
			} else if (facingLock == "NE") {
				if (stickInput.x > 0 && stickInput.y > 0) {
					moveBackInt = -1;
				} else if (stickInput.x < 0 && stickInput.y < 0) {
					moveBackInt = 1;
				}
			} else if (facingLock == "SE") {
				if (stickInput.x < 0 && stickInput.y > 0) {
					moveBackInt = -1;
				} else if (stickInput.x > 0 && stickInput.y < 0) {
					moveBackInt = 1;
				}
			} else if (facingLock == "SW") {
				if (stickInput.x < 0 && stickInput.y < 0) {
					moveBackInt = -1;
				} else if (stickInput.x > 0 && stickInput.y > 0) {
					moveBackInt = 1;
				}
			} else if (facingLock == "NW") {
				if (stickInput.x > 0 && stickInput.y < 0) {
					moveBackInt = -1;
				} else if (stickInput.x < 0 && stickInput.y > 0) {
					moveBackInt = 1;
				}
			}



			//0 = left, 1 = right
			RaycastHit shimmyBoundsLeft;
			RaycastHit shimmyBoundsRight;

			//if (shimmy!=shimmyLock.unlocked)
			//print ("Lock to: " + shimmy);
			switch (shimmy) {
			//Self MOVEMENT (if script attached to parent)
			case shimmyLock.xlockedFacingNorth:
			case shimmyLock.zlockedFacingNorth:

				if (moveBackInt == 1) {

					Debug.DrawRay(leftShimmyReach.transform.position, transform.TransformDirection(Vector3.down) * 5, Color.yellow, 1);
					if (Physics.Raycast (leftShimmyReach.transform.position, Vector3.down, out shimmyBoundsLeft, 5)) {
						if (shimmyBoundsLeft.collider.gameObject != this.gameObject.GetComponent<Climber> ().climbingObj) {
							print (shimmyBoundsLeft.collider + " - " + this.gameObject.GetComponent<Climber> ().climbingObj);
							moveBackInt = 0;
						}
					} else {
						moveBackInt = 0;
					}
				}

				if (moveBackInt == -1) {

					Debug.DrawRay (rightShimmyReach.transform.position, transform.TransformDirection (Vector3.down) * 5, Color.red, 1);
					if (Physics.Raycast (rightShimmyReach.transform.position, Vector3.down, out shimmyBoundsRight, 5)) {
						if (shimmyBoundsRight.collider.gameObject != this.gameObject.GetComponent<Climber> ().climbingObj) {
							moveBackInt = 0;
						}
					} else {
						moveBackInt = 0;
					}
				}

				this.gameObject.transform.Translate (Vector3.left * Time.deltaTime * moveSpeed * moveBackInt);
				break;
			case shimmyLock.xlockedFacingSouth:
			case shimmyLock.zlockedFacingSouth:
				if (moveBackInt == -1) {

					Debug.DrawRay(leftShimmyReach.transform.position, transform.TransformDirection(Vector3.down) * 5, Color.yellow, 1);
					if (Physics.Raycast (leftShimmyReach.transform.position, Vector3.down, out shimmyBoundsLeft, 5)) {
						if (shimmyBoundsLeft.collider.gameObject != this.gameObject.GetComponent<Climber> ().climbingObj) {
							print (shimmyBoundsLeft.collider + " - " + this.gameObject.GetComponent<Climber> ().climbingObj);
							moveBackInt = 0;
						}
					} else {
						moveBackInt = 0;
					}
				}

				if (moveBackInt == 1) {

					Debug.DrawRay (rightShimmyReach.transform.position, transform.TransformDirection (Vector3.down) * 5, Color.red, 1);
					if (Physics.Raycast (rightShimmyReach.transform.position, Vector3.down, out shimmyBoundsRight, 5)) {
						if (shimmyBoundsRight.collider.gameObject != this.gameObject.GetComponent<Climber> ().climbingObj) {
							moveBackInt = 0;
						}
					} else {
						moveBackInt = 0;
					}
				}
				this.gameObject.transform.Translate (Vector3.right * Time.deltaTime * moveSpeed * moveBackInt);
				break;

			case shimmyLock.unlocked: //Moving under normal circumstances
			default: //Moving under normal circumstances
				this.gameObject.transform.Translate (Vector3.forward * Time.deltaTime * moveSpeed * moveBackInt);
				break;
			}

		} else {
			characterLocked--;
		}
	}

	public void lockCharacter(int duration) {
		characterLocked = duration;
	}

	public void restrictMovement() {

		//This no longer works.
		//Instead we will need to set player position, rotate to face this object and then lock rotation
		rotatorLock = 0.0f;
	}

	public void freeMovement() {
		rotatorLock = 1.0f;
	}

	//This checks if the player is currently standing on a surface
	//This is so that if the player is dragging something, it does not suspend them in midair
	public bool checkFloor() {
		RaycastHit hit;
		if (Physics.Raycast (this.transform.position, Vector3.down, out hit, 1)) {
			return true;
		} else
			return false;
	}
}
