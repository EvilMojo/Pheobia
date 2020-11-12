using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : Activatable {

	public override void activate() {
		
		animator.SetBool ("open", true);

		this.gameObject.transform.Find ("Door_Open").GetComponent<BoxCollider> ().enabled = false;

		GameObject.Find ("CameraTarget").GetComponent<CameraControl>().sendCameraTo(this.gameObject, 360);
	}

	public override void deactivate() {

	}
}
