using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : Activatable {

	public override void activate() {

		if (activated) {
			animator.speed = 1000.0f;
		}

		activated = true;
		animator.SetBool ("open", true);
		this.gameObject.transform.Find ("Door").GetComponent<BoxCollider> ().enabled = false;
		GameObject.Find ("CameraTarget").GetComponent<CameraControl>().sendCameraTo(this.gameObject, 360);
	}

	public override void deactivate() {

	}

}
