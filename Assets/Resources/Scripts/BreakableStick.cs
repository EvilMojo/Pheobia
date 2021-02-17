using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableStick : MonoBehaviour {

	void OnCollisionEnter(Collision col) {
		print (col.gameObject.name);
		if (col.gameObject.GetComponent<Item> () != null) {
			
			this.transform.Find ("default").gameObject.GetComponent<MeshRenderer> ().enabled = false;
			Destroy (this.gameObject.GetComponent<BoxCollider> ());

			GameObject top = this.transform.Find ("sticktop").gameObject;
			GameObject bot = this.transform.Find ("stickbot").gameObject;


			Vector3 vel = new Vector3 (col.gameObject.GetComponent<Rigidbody> ().velocity.x/2,
				col.gameObject.GetComponent<Rigidbody> ().velocity.y/2,
				col.gameObject.GetComponent<Rigidbody> ().velocity.z/2);

			top.AddComponent<Rigidbody> ();
			top.GetComponent<Rigidbody> ().AddForce(vel);

			bot.AddComponent<Rigidbody> ();
			bot.GetComponent<Rigidbody> ().AddForce(vel);

			GameObject bridge = GameObject.Find ("woodbridge");
			bridge.GetComponent<Activatable> ().activate ();

		}
	}
}
