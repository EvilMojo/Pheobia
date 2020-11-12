using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearTrap : Activatable {

	public float force;
	public GameObject limb;
	public Vector3 launchPosition;
	// Use this for initialization
	void Awake () {
		force = 100.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void activate () {
		launchPosition = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		this.gameObject.AddComponent<Rigidbody> ();
		this.gameObject.GetComponent<Rigidbody> ().AddForce (transform.rotation * Vector3.forward * force, ForceMode.Impulse);
	}

	public override void deactivate() {

	}

	//Spear tip kills the player	
	void OnTriggerEnter (Collider other) {
		

		//other.gameObject.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity;
		/*this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		this.gameObject.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		this.gameObject.transform.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		Destroy (this.gameObject.GetComponent<Rigidbody> ());
		Destroy (this.gameObject.transform.GetChild (0).gameObject.GetComponent<BoxCollider> ());
		this.gameObject.transform.SetParent (other.gameObject.transform);*/

		//this.transform.parent = other.gameObject.transform;

	}

	/*void OnCollisionStay (Collision other) {
		if (other.gameObject.name == "playerCharacter") {
			other.transform.SetPositionAndRotation (this.transform.position, other.transform.rotation);
		}
	}*/

	void OnCollisionEnter (Collision other) {


		if (other.gameObject.name == "playerCharacter") {
			//this.transform.SetParent(other.gameObject.transform);
			//other.transform.SetParent (transform);

			//other.gameObject.GetComponent<PlayerMovement> ().enabled = false;

			Destroy(this.GetComponent<Collider> ());

			//This launches the player in a given direction depending on the spear
			Vector3 forceVector = new Vector3 (this.transform.position.x - launchPosition.x, 
				0, 
				this.transform.position.z - launchPosition.z);

			if (Mathf.Abs (launchPosition.x - other.gameObject.transform.position.x) > Mathf.Abs (launchPosition.z - other.gameObject.transform.position.z)) {
				forceVector = new Vector3 (forceVector.x, 0, 0);
			} else {
				forceVector = new Vector3 (0, 0, forceVector.z);
			}


			//print ("DIR" + forceVector);
			other.gameObject.GetComponent<PlayerManager> ().commitDie (forceVector, force);
			other.gameObject.GetComponent<PlayerMovement> ().enabled = false;
			//Destroy (other.gameObject.GetComponent<Rigidbody> ());


			//this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			//this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			//this.gameObject.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			//this.gameObject.transform.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			//Destroy (this.gameObject.GetComponent<Rigidbody> ());
			//Destroy (this.gameObject.transform.GetChild (0).gameObject.GetComponent<BoxCollider> ());
		} else if (other.gameObject.tag != "PlayerArmature") {

			print ("Collided with wall");
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			this.gameObject.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.transform.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			Destroy (this.gameObject.GetComponent<Rigidbody> ());
			Destroy (this.gameObject.transform.GetChild (0).gameObject.GetComponent<BoxCollider> ());
		}


		//if (other.gameObject.tag != "PlayerArmature") {

			/*if (transform.Find ("playerCharacter")) {
				print ("GOT PLAYER");
				GameObject player = transform.Find ("playerCharacter").gameObject;
				//player.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
				//player.GetComponent<Rigidbody> ().isKinematic = true;
				//player.GetComponent<Rigidbody> ().useGravity = false;
				player.transform.Find ("playerMesh/Armature/Root/Torso").GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;
				player.transform.Find("playerMesh/Armature/Root/Torso").GetComponent<Rigidbody> ().isKinematic = true;
				player.transform.Find("playerMesh/Armature/Root/Torso").GetComponent<Rigidbody> ().useGravity = false;
				player.GetComponent<PlayerManager> ().commitDie ();
				player.GetComponent<PlayerMovement> ().enabled = false;
			}*/
		/*
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			this.gameObject.transform.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.transform.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			Destroy (this.gameObject.GetComponent<Rigidbody> ());
			Destroy (this.gameObject.transform.GetChild (0).gameObject.GetComponent<BoxCollider> ());
			//this.gameObject.transform.SetParent (other.gameObject.transform);
		}

		if (other.gameObject.GetComponent<Monster> () != null) {
			other.gameObject.GetComponent<Monster> ().takeDamage (1.0f);
		} else {
			print ("KILLED: " + other.gameObject.name);
		}*/
		//Spear lodges into the player
		/*
		this.gameObject.transform.SetParent (other.gameObject.transform);
		this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		this.gameObject.transform.parent.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		this.gameObject.transform.parent.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		Destroy(this.gameObject.GetComponent<Rigidbody>());
		Destroy(this.gameObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider>());*/
	}
}
