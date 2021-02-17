using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Activatable {

	public List<GameObject> targetList;
	public int activations; //Set from inspector

	void OnTriggerEnter(Collider collision) {
		if (!activated) {
			activate ();
		}
	}

	void OnTriggerExit(Collider collision) {
		if (activated) {
			deactivate ();
		}
	}

	public override void activate() {

		print ("Activated??" + activated);

		if (activated) {
			animator.speed = 1000.0f;
			animator.SetBool ("active", true);
			foreach (GameObject o in targetList) {
				if (o.GetComponent<Activatable> () != null) {
					o.GetComponent<Activatable> ().activate ();
				}
			}
		}

		if (activations != 0) {
			activations--;
			activated = true;
			animator.SetBool ("active", true);
			foreach (GameObject o in targetList) {
                if (o != null)
                {
                    if (o.GetComponent<Activatable>() != null)
                    {
                        o.GetComponent<Activatable>().activate();
                    }
                }
			}
		}
	}

	public override void deactivate() {
		//If there are activations left, switch returns to normal
		if (activations > 0) {
			animator.SetBool ("active", false);
		}
	}
}
