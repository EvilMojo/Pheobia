using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	public virtual void executeInteraction() {
		print ("Executing main interactable");
	}


	void OnCollisionEnter(Collision col) {
		//print ("Collision");
	}

	void OnTriggerEnter(Collider other) {
		//print (other.name);
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
			other.gameObject.GetComponent<PlayerInteraction> ().setTarget (this.gameObject);
		} else {

			//print ("No PlayerInteraction Class Found");

		}

	}

	void OnTriggerExit(Collider other) {
		//print ("Leaving");
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
			other.gameObject.GetComponent<PlayerInteraction> ().setTarget (null);
		} else {
			//print ("No PlayerInteraction Class Found");
		}
	}
}
