using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : MonoBehaviour {
	
	void OnTriggerEnter(Collider other) {
		print (other.name + "ready to climb");
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
		} else {
			//print ("No PlayerInteraction Class Found");
		}

	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
		} else {
			//print ("No PlayerInteraction Class Found");

		}

	}
}
