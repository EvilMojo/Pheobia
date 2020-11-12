using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpearAttack : MonsterAttack {

	public float throwVelocity = 5000.0f;
	public BoxCollider owner;

	void start () {
		//throwVelocity = 5000.0f;
	}

	void Update () {
	}

	public void launch(GameObject owner) {
		this.owner = owner.GetComponent<BoxCollider>();
		this.gameObject.AddComponent<Rigidbody> ();
		//print (Vector3 this.gameObject.forward);
		this.gameObject.GetComponent<Rigidbody> ().AddForce (transform.rotation * Vector3.forward * throwVelocity);
		this.gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.up * 100);

	}

	void OnTriggerEnter (Collider other) {
		//Spear tip kills the player	
		if (other != owner) {
			if (other.name == "playerMesh" || other.name == "monster") {
				this.gameObject.transform.SetParent (other.gameObject.transform);
				if (other.name == "playerMesh") {
					other.transform.parent.gameObject.GetComponent<PlayerManager> ().commitDie ();
				}
			}
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			this.gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			Destroy (this.gameObject.GetComponent<Rigidbody> ());
			Destroy (this.gameObject.transform.GetChild (0).gameObject.GetComponent<BoxCollider> ());
		}
	}

}
