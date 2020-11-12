using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : MonoBehaviour {
	
	public Animator animator;
	public bool activated;

	void Awake() {

		activated = false;
		animator = this.GetComponent<Animator> ();
		if (animator != null) {
			animator.SetBool ("active", false);
		}
	}

	public abstract void activate ();
	public abstract void deactivate ();

}
