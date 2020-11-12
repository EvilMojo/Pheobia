using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climber : MonoBehaviour {

	//Maybe make this a list in case future climbables are close together
	public GameObject target;
	public Vector3 climbingDirection;
	public GameObject climbingObj;
	//public GameObject climbAxisLock;
	public string facing;
	public int duration;
	public float climbTime;		//Time it takes to climb, multiplicative per category //Small = 1, Medium = 2, High = 3
	public float climbDistance; 	//Measures how high one category of distance goes //Small = 1, Medium = 2, High = 3
	public bool hanging;

	public GameObject player;

	// Use this for initialization
	void Start () {
		climbTime = 10.0f;
		climbDistance = 4f;
		player = this.gameObject;
		climbingObj = new GameObject ();
		climbingObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		climbingObj.GetComponent<MeshRenderer> ().material.color = Color.yellow;

		//climbAxisLock = new GameObject ();
		//climbAxisLock = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		//climbAxisLock.GetComponent<MeshRenderer>().

		hanging = false;

	}

	// Update is called once per frame
	void Update () {

		//climbingObj = null;
		//string climbHeight = drawClimbingRay (player.transform);
		/*if (climbAxisLock == null) {
			climbAxisLock = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			climbAxisLock.name = "AxisLock";
			climbAxisLock.GetComponent<MeshRenderer> ().material.color = Color.yellow;
			climbAxisLock.transform.position = player.transform.position;
			climbAxisLock.transform.position = new Vector3 (0, 0, 0);
		}*/

		if (target != null) {
			climbingDirection = this.transform.position - target.transform.position;

			if(Mathf.Abs (climbingDirection.x) - Mathf.Abs (climbingDirection.z) > 1.0f || Mathf.Abs (climbingDirection.z) - Mathf.Abs (climbingDirection.x) > 1.0f) {
				if (Mathf.Abs (climbingDirection.x) < Mathf.Abs (climbingDirection.z)) {
					//print ("closer on Z axis");
					if (climbingDirection.z > 0) {
						facing = "posZ";
					} else {
						facing = "negZ";
					}
				} else {
					if (climbingDirection.x > 0) {
						facing = "posX";
					} else {
						facing = "negX";
					}
					//print ("closer on X axis");
				}
			}
		}

		if (duration > 0) {
			//Climbing...
			duration--;

			if (duration == 0) {
				player.transform.position = new Vector3 (target.transform.position.x, target.transform.parent.GetComponent<BoxCollider>().bounds.size.y + 1.0f, target.transform.position.z);
				player.GetComponent<PlayerMovement> ().characterFrozen = false;
				this.target = null;
				//Move player to new location
			}
		}
	}

	public void drop() {
		this.gameObject.GetComponent<Climber>().hanging = false;
		this.gameObject.GetComponent<PlayerMovement> ().shimmy = PlayerMovement.shimmyLock.unlocked;
		this.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;
		this.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
		this.climbingObj = null;
	}

	public void cling() {
		this.gameObject.GetComponent<Climber>().hanging = true;
		this.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
	}

	public void processClimb(string category, GameObject climber) {

		//set rotation to face ledge
		float direction = roundToNearestDirection (climber);
		climber.transform.eulerAngles = new Vector3 (0, direction, 0);

		if (category == "low" || category == "medium") {
			hoist (climber, climbingObj);
		} else if (category == "high") {
			if (hanging) {
				hoist (climber, climbingObj);
			} else {
				grabLedge (climber);
				cling ();
			}
		} else if (category == "UNREACHABLE") {

		} else if (category == "none") {

		}
	}

	public void grabLedge(GameObject climber) {
		
		//set position to be exactly where ledge wall item is
		//climbAxisLock.transform.position = climbingObj.transform.position + new Vector3(0, climbingObj.GetComponent<Collider>().bounds.size.y/2, 0);

		//set height to be ledge height (Diff between player height and ledge height)
		//Need to factor in ledges that do not touch the ground (use position + height)
		float heightDiff = calculateHeightDiff(climber, climbingObj);
		climber.transform.Translate(new Vector3(0, heightDiff, 0));

	}

	public float roundToNearestDirection(GameObject climber) {

		//true = x | false = z;

		float rot = climber.transform.rotation.eulerAngles.y;

		if (rot > 0.0f && rot <= 45.0f || rot > 315.0f && rot <= 360) {
			rot = 0.0f;
			climber.GetComponent<PlayerMovement> ().shimmy = PlayerMovement.shimmyLock.xlockedFacingNorth;
		} else if (rot > 45.0f && rot <= 135.0f) {
			rot = 90.0f;
			climber.GetComponent<PlayerMovement> ().shimmy = PlayerMovement.shimmyLock.zlockedFacingSouth;
		} else if (rot > 135.0f && rot <= 225.0f) {
			rot = 180.0f;
			climber.GetComponent<PlayerMovement> ().shimmy = PlayerMovement.shimmyLock.xlockedFacingSouth;
		} else if (rot > 225.0f && rot <= 315.0f) {
			rot = 270.0f;
			climber.GetComponent<PlayerMovement> ().shimmy = PlayerMovement.shimmyLock.zlockedFacingNorth;
		}
			
		return rot;
	}

	public float calculateHeightDiff (GameObject player, GameObject climbable) {

		//Origin point of player model is at bottom of player object
		//Origin point of climbable object is at centre of climbable object

		//We want object (origin + height/2) - player (origin + player height)
		float playerTop = player.transform.position.y + player.GetComponent<Collider>().bounds.size.y;
		float climbableTop = climbable.transform.position.y + (climbable.GetComponent<Collider>().bounds.size.y/2);

		//if the climbed object is lower than the player, don't climb. If it's higher, raise player to its top
		if (climbableTop - playerTop > 0) {
			return climbableTop - playerTop;
		}
		else {
			return 0;
		}
	}

	public void hoist(GameObject climber, GameObject climbable) {
		float climbableTop = climbable.transform.position.y + (climbable.GetComponent<Collider>().bounds.size.y/2);
		climber.transform.Translate (new Vector3 (0, climbableTop + .3f, climber.GetComponent<Collider> ().bounds.size.x));
		drop();
	}

	/*public void deprecatedClimb() {
		if(target.GetComponent<Climbable>() != null) {

			Transform rotateTarget = this.transform.gameObject.transform.parent.transform;

			player.GetComponent<PlayerMovement> ().characterFrozen = true;

			player.transform.LookAt (new Vector3(target.transform.position.x, player.transform.position.y, target.transform.position.z));

			float playerOffset = 0.0f;

			if (player.transform.position.z > this.gameObject.transform.position.z) {
				playerOffset = (this.transform.gameObject.GetComponent<BoxCollider>().bounds.size.z) - (player.GetComponent<BoxCollider>().bounds.size.z);
			} else if (player.transform.position.z < this.gameObject.transform.position.z) {
				playerOffset = -(this.transform.gameObject.GetComponent<BoxCollider>().bounds.size.z) + (player.gameObject.GetComponent<BoxCollider>().bounds.size.z);
			}
			if (player.transform.position.x > this.gameObject.transform.position.x) {
				playerOffset = (this.transform.gameObject.GetComponent<BoxCollider>().bounds.size.x) - (player.gameObject.GetComponent<BoxCollider>().bounds.size.z);
			} else if (player.transform.position.x < this.gameObject.transform.position.x) {
				playerOffset = -(this.transform.gameObject.GetComponent<BoxCollider>().bounds.size.x) + (player.gameObject.GetComponent<BoxCollider>().bounds.size.z);
			}

			duration = (int)climbTime;
			//target.GetComponent<Climbable>().executeInteraction();
		} else { 
			print ("No climber on Interactable Object found");
		}
	}

	public void setTarget(GameObject target) {
		this.target = target;
		print (this.name + " is now targetting " + this.target.name);
	}


	void OnTriggerEnter(Collider other) {
		print (this.gameObject.name + " is ready to climb " + other.name);
		if (other.gameObject.GetComponent<Climbable> () != null) {
			this.target = other.gameObject;
			print (other + " is a valid climbable object");
			print (climbingDirection);
			print (other.name + "ready to climb");
		} else {
			//print ("No climber Class Found");

		}

	}

	void OnTriggerExit(Collider other) {
		//print (this.gameObject.name + " is ready to climb " + other.name);
		if (other.gameObject.GetComponent<Climbable> () != null && duration == 0) {
			this.target = null;
			//print (other.name + "ready to climb");
		} else {

		}

	}*/

	public string drawClimbingRay(Transform climberTransform) {

		string category = "None"; //No climbable object found
	/*	GameObject grabHeight = GameObject.CreatePrimitive(PrimitiveType.Plane);
		grabHeight.GetComponent<MeshRenderer> ().material.color = Color.white;
		grabHeight.transform.SetPositionAndRotation (this.gameObject.transform.position, this.gameObject.transform.rotation);
		Destroy (grabHeight.GetComponent<Collider> ());
		grabHeight.AddComponent<SelfDestructTimer> ();
		grabHeight.GetComponent<SelfDestructTimer> ().setTimer (.1f);
		grabHeight.transform.localScale = new Vector3(2, 1, .20f);*/

		float tz = player.GetComponent<BoxCollider> ().size.z;
		float tx = player.GetComponent<BoxCollider> ().size.x/2;

		GameObject centreReach = new GameObject();
		GameObject leftReach = new GameObject();
		GameObject rightReach = new GameObject();

		centreReach.transform.SetPositionAndRotation (climberTransform.position, climberTransform.rotation);
		leftReach.transform.SetPositionAndRotation (centreReach.transform.position, centreReach.transform.rotation);
		rightReach.transform.SetPositionAndRotation (centreReach.transform.position, centreReach.transform.rotation);

		centreReach.transform.Translate (new Vector3(0,climbDistance*4,tz));
		leftReach.transform.Translate (new Vector3(-tx,climbDistance*4,tz));
		rightReach.transform.Translate (new Vector3(tx,climbDistance*4,tz));

		Debug.DrawRay(centreReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.magenta, 1);
		Debug.DrawRay(leftReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.magenta, 1);
		Debug.DrawRay(rightReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.magenta, 1);

		RaycastHit centreHit;
		if (Physics.Raycast (centreReach.transform.position, Vector3.down, out centreHit, climbDistance) ||
			Physics.Raycast (leftReach.transform.position, Vector3.down, out centreHit, climbDistance) || 
			Physics.Raycast (rightReach.transform.position, Vector3.down, out centreHit, climbDistance)) {
			//if (centreHit.collider != null)
			//	print ("TooHigh: " + centreHit.collider.gameObject.name);
			float y = centreHit.transform.position.y + centreHit.collider.bounds.size.y/2;
			//grabHeight.transform.position = new Vector3(centreHit.transform.position.x, y, centreHit.transform.position.z);
			//grabHeight.GetComponent<MeshRenderer> ().material.color = Color.magenta;
			//climbingObj = grabHeight;
			print (centreHit.collider.gameObject.name);
			category = category+"UNREACHABLE";
		}

		centreReach.transform.Translate (new Vector3(0,-climbDistance,0));
		leftReach.transform.Translate (new Vector3(0,-climbDistance,0));
		rightReach.transform.Translate (new Vector3(0,-climbDistance,0));

		Debug.DrawRay(centreReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.red, 1);
		Debug.DrawRay(leftReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.red, 1);
		Debug.DrawRay(rightReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.red, 1);

		if (Physics.Raycast (centreReach.transform.position, Vector3.down, out centreHit, climbDistance) ||
			Physics.Raycast (leftReach.transform.position, Vector3.down, out centreHit, climbDistance) || 
			Physics.Raycast (rightReach.transform.position, Vector3.down, out centreHit, climbDistance)) {
			//if (centreHit.collider != null)
			print ("High: " + centreHit.collider.gameObject.name);
			float y = centreHit.transform.position.y + centreHit.collider.bounds.size.y/2;
			//grabHeight.transform.position = new Vector3(centreHit.transform.position.x, y, centreHit.transform.position.z);
			//grabHeight.GetComponent<MeshRenderer> ().material.color = Color.red;

			//climbingObj = grabHeight;
			//print (centreHit.collider.gameObject.name);
			climbingObj = centreHit.collider.gameObject;
			climbingObj.name = "highObj";
			category = category+"High";
			//climbingObj.transform.SetPositionAndRotation(centreHit.point, Quaternion.identity);
		}

		centreReach.transform.Translate (new Vector3(0,-climbDistance,0));
		leftReach.transform.Translate (new Vector3(0,-climbDistance,0));
		rightReach.transform.Translate (new Vector3(0,-climbDistance,0));

		Debug.DrawRay(centreReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.green, 1);
		Debug.DrawRay(leftReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.green, 1);
		Debug.DrawRay(rightReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.green, 1);

		if (Physics.Raycast (centreReach.transform.position, Vector3.down, out centreHit, climbDistance) ||
			Physics.Raycast (leftReach.transform.position, Vector3.down, out centreHit, climbDistance) || 
			Physics.Raycast (rightReach.transform.position, Vector3.down, out centreHit, climbDistance)) {
			//if (centreHit.collider != null)
			//	print ("Medium: " + centreHit.collider.gameObject.name);
			float y = centreHit.transform.position.y + centreHit.collider.bounds.size.y/2;
			//grabHeight.transform.position = new Vector3(centreHit.transform.position.x, y, centreHit.transform.position.z);
			//grabHeight.GetComponent<MeshRenderer> ().material.color = Color.green;
			//climbingObj = grabHeight;
			climbingObj = centreHit.collider.gameObject;
			climbingObj.name = "medObj";
			category = category+"Medium";
		}

		centreReach.transform.Translate (new Vector3(0,-climbDistance,0));
		leftReach.transform.Translate (new Vector3(0,-climbDistance,0));
		rightReach.transform.Translate (new Vector3(0,-climbDistance,0));

		Debug.DrawRay(centreReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.blue, 1);
		Debug.DrawRay(leftReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.blue, 1);
		Debug.DrawRay(rightReach.transform.position, transform.TransformDirection(Vector3.down) * climbDistance, Color.blue, 1);

		if (Physics.Raycast (centreReach.transform.position, Vector3.down, out centreHit, climbDistance) ||
			Physics.Raycast (leftReach.transform.position, Vector3.down, out centreHit, climbDistance) ||
			Physics.Raycast (rightReach.transform.position, Vector3.down, out centreHit, climbDistance)) {
			//if (centreHit.collider != null)
			//	print ("Low: " + centreHit.collider.gameObject.name);
			float y = centreHit.transform.position.y + centreHit.collider.bounds.size.y/2;
			//grabHeight.transform.position = new Vector3(centreHit.transform.position.x, y, centreHit.transform.position.z);
			//grabHeight.GetComponent<MeshRenderer> ().material.color = Color.blue;
			//climbingObj = grabHeight;
			climbingObj = centreHit.collider.gameObject;
			climbingObj.name = "lowObj";
			category = category + "Low";
		}
			
		Destroy (centreReach);
		Destroy (leftReach);
		Destroy (rightReach);

		print (climbingObj);

		if (category.Contains ("UNREACHABLE")) {
			category = "UNREACHABLE";
		} else if (category.Contains ("High")) {
			category = "high";
		} else if (category.Contains ("Medium")) {
			category = "medium";
		} else if (category.Contains ("Low")) {
			category = "low";
		} else
			category = "none";

		//print (category);
		return category;
	}

}
