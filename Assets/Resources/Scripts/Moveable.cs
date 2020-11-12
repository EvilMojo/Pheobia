using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Moveable : Interactable {

	public GameObject player;
	public bool gripped;
	public string axis; //needs to be preset

	//Movement Properties
	public float moveSpeedMultiplier;

	// Use this for initialization
	void Start () {
		gripped = false;
		moveSpeedMultiplier = .5f;
	}

	// Update is called once per frame
	void Update () {
		//print (Mathf.Abs(this.transform.parent.gameObject.GetComponent<Rigidbody>().velocity.y));
		if (transform.parent.gameObject.GetComponent<Rigidbody> () != null) {
			if (gripped && Mathf.Abs (transform.parent.gameObject.GetComponent<Rigidbody> ().velocity.y) >= 2.0f) {
				//print ("releasing");
				release ();
			}
		} else if (transform.parent.gameObject.GetComponent<Rigidbody> () == null) {

		}
	}

	public override void executeInteraction (){ 
		if (gripped) {
			release ();
		} else {
			//print (this.transform.parent.gameObject.GetComponent<Rigidbody> ().velocity.y);
			if (this.transform.parent.gameObject.GetComponent<Rigidbody> () != null &&
			    this.transform.parent.gameObject.GetComponent<Rigidbody> ().velocity.y <= 0) {
				grip ();
			} else if (this.transform.parent.gameObject.GetComponent<Rigidbody> () == null) {
				grip ();
			}
		}
	}
		
	void OnCollisionEnter(Collision col) {
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
			player = other.gameObject;
			player.GetComponent<PlayerInteraction> ().setTarget (this.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
			other.gameObject.GetComponent<PlayerInteraction> ().setTarget (null);
		} 
	}

	public void grip() {

		//So uhhhhh we do something super weird and make this the parent of the player and then lock the X and Z axis that way
		//player.transform.parent.SetParent(this.transform);

		//Need to do the following
		//Determine what side of movable the player is on
		//Move player to centre of this axis
		//rotate player to face object

		float playerOffset = 0.0f;

		if (axis == "z") {
			if (player.transform.position.z > this.gameObject.transform.position.z) {
				player.GetComponent<PlayerMovement> ().facingLock = "SW";
				playerOffset = (this.transform.parent.gameObject.GetComponent<BoxCollider>().bounds.size.z) - (player.GetComponent<BoxCollider>().bounds.size.z);
			} else if (player.transform.position.z < this.gameObject.transform.position.z) {
				player.GetComponent<PlayerMovement> ().facingLock = "NE";
				playerOffset = -(this.transform.parent.gameObject.GetComponent<BoxCollider>().bounds.size.z) + (player.gameObject.GetComponent<BoxCollider>().bounds.size.z);
			}
		} else {
			if (player.transform.position.x > this.gameObject.transform.position.x) {
				player.GetComponent<PlayerMovement> ().facingLock = "SE";
				playerOffset = (this.transform.parent.gameObject.GetComponent<BoxCollider>().bounds.size.x) - (player.gameObject.GetComponent<BoxCollider>().bounds.size.z);
			} else if (player.transform.position.x < this.gameObject.transform.position.x) {
				player.GetComponent<PlayerMovement> ().facingLock = "NW";
				playerOffset = -(this.transform.parent.gameObject.GetComponent<BoxCollider>().bounds.size.x) + (player.gameObject.GetComponent<BoxCollider>().bounds.size.z);
			}
		}

		if (axis == "z") {
			player.transform.position = new Vector3(this.gameObject.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z+playerOffset);
		} else {
			player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z);
		}

		player.transform.LookAt(new Vector3(this.gameObject.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z));

		player.GetComponent<PlayerMovement> ().restrictMovement ();
		this.transform.parent.SetParent(player.transform);
		Destroy (this.transform.parent.gameObject.GetComponent<Rigidbody> ());

		//this.transform.parent.SetParent (player.transform);
		gripped = true;
	}

	public void release() {

		//print ("Release");
		transform.parent.gameObject.GetComponent<DetectFloor> ().release ();

		/*player.GetComponent<PlayerMovement> ().freeMovement ();
		player.GetComponent<PlayerMovement> ().facingLock = "None";
		this.transform.parent.SetParent(null);

		if (this.transform.parent.gameObject.GetComponent<Rigidbody> () != null) {
			Rigidbody rbody = this.transform.parent.gameObject.AddComponent<Rigidbody> ();

			rbody.freezeRotation = true;
			rbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;

			rbody.mass = 20;
		}

		SceneManager.MoveGameObjectToScene (transform.parent.gameObject, SceneManager.GetActiveScene ());

		gripped = false;*/
	}
}