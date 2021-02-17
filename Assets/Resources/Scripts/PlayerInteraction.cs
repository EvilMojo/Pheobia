using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	//Maybe make this a list in case future interactables are close together
	public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public bool interact() {
		bool interactionfound = false;
        //The interactable (target) is a child of the core item object
        //print ("Targetto: " + target.name);
        //print("Interacting: " + target.name);
		if (target != null) {
			if (target.GetComponent<Interactable> () != null && target.activeSelf) {
				target.GetComponent<Interactable> ().executeInteraction ();
				interactionfound = true;
			}
		}

		return interactionfound;
	}

	public void setTarget(GameObject target) {
		this.target = target;
	}


	void OnTriggerEnter(Collider other) {
		//print (other.gameObject);
		if (other.gameObject.GetComponent<PlayerInteraction> () != null) {
			//other.gameObject.GetComponent<PlayerInteraction> ().setTarget (this.gameObject);
			//print (other.name + "ready to climb");
		} else {
			//print ("No PlayerInteraction Class Found");
		}
	}
}
