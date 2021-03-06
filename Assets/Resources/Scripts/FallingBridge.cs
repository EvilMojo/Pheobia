using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridge : Activatable {

	public override void activate() {

		activated = true;
		animator.SetBool ("open", true);

		this.gameObject.GetComponent<BoxCollider>().enabled = true;
		GameObject.Find ("CameraTarget").GetComponent<CameraControl>().sendCameraTo(this.gameObject, 100);
	}

	public override void deactivate() {

	}

}
