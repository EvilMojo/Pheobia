using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour {

	protected GameObject root;
	protected Collider[] childColliders;
	protected Rigidbody[] childRigidbodies;

	// Use this for initialization
	void Awake () {
		root = this.gameObject.transform.Find ("playerMesh/Armature/Root").gameObject;
		childColliders = this.transform.Find("playerMesh/Armature").gameObject.GetComponentsInChildren<Collider> ();
		childRigidbodies = this.transform.Find("playerMesh/Armature").gameObject.GetComponentsInChildren<Rigidbody> ();
	}

	void Start() {
		activateRagDoll (false);
	}

	void Update() {
		/*if (Input.GetButtonDown("Interact")) {
			Vector3 force = Vector3.forward * 100.0f;
			root.GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);
			foreach (Rigidbody r in childRigidbodies) {
				r.AddForce (force, ForceMode.Impulse);
			}
		}*/
	}

	public void activateRagDoll(bool active) {


		foreach (Collider c in childColliders) {
			c.enabled = active;
		}
		foreach (Rigidbody r in childRigidbodies) {
			r.detectCollisions = active;
			r.isKinematic = !active;
		}

		//Disable animator
		//
		if (this.gameObject.GetComponent<Rigidbody> () != null) {
			this.gameObject.GetComponent<Rigidbody> ().detectCollisions = !active;
			this.gameObject.GetComponent<Rigidbody> ().isKinematic = active;
		}
		this.gameObject.GetComponent<BoxCollider>().enabled = !active;
		this.gameObject.GetComponent<PlayerMovement> ().enabled = !active;
		this.gameObject.GetComponent<PlayerInteraction> ().enabled = !active;
		this.gameObject.GetComponent<PlayerInteraction> ().enabled = !active;

		//this.gameObject.transform.Find ("playerMesh/ClimbingReach").gameObject.GetComponent<BoxCollider> ().enabled = !active;
		//this.gameObject.transform.Find ("playerMesh/ClimbingReach").gameObject.GetComponent<Climber> ().enabled = !active;

	}

	public void applyForceToRagDoll(Vector3 direction, float force) {
		root.GetComponent<Rigidbody> ().useGravity = false;
		root.GetComponent<Rigidbody> ().isKinematic = false;
		//root.GetComponent<Rigidbody> ().AddForce (new Vector3(0,2000,0));
		//Vector3 force = Vector3.forward * 100.0f;
		/*root.GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);
		foreach (Rigidbody r in childRigidbodies) {
			r.AddForce (force, ForceMode.Impulse);
		}*/

		//Really ham up the force, this is likely a very uni-directional force.
		if (direction.x == 0 && direction.y == 0) {
			int n = 0;
			if (direction.z < 0)
				n = -1;
			else n = 1;
			direction = new Vector3 (0, 0, n);
		} else if (direction.z == 0 && direction.y == 0) {
			int n = 0;
			if (direction.x < 0)
				n = -1;
			else n = 1;
			direction = new Vector3 (n, 0, 0);
		}

		print ("FORCE: " + direction * force);
		root.GetComponent<Rigidbody> ().AddForce (direction * force, ForceMode.Impulse);
		foreach (Rigidbody r in childRigidbodies) {
			r.AddForce (direction * force, ForceMode.Impulse);
		}
	}
}
