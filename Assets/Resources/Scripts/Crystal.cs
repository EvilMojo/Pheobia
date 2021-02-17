using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Item {

	/*GameObject player;
	public override void executeInteraction() {
		//print ("PICKUP THE ITEM");
		if (player == null)
			player = GameObject.Find ("playerCharacter");
		if (this.gameObject.GetComponent<Rigidbody> () != null) {
			if (player.GetComponent<PlayerManager> ().inventory [0] == null) {
				pickupItem ();
			} else {
				player.GetComponent<PlayerManager> ().inventory [0].GetComponent<Item> ().dropItem ();
				pickupItem ();
			}
		} else {
			dropItem ();
		}
	}*/

	// Update is called once per frame
	public override void useItem() {

		GameObject player = GameObject.Find ("playerCharacter");

		GameObject rayStart = new GameObject ();
		rayStart.transform.SetPositionAndRotation (player.transform.position, player.transform.rotation);
		rayStart.transform.Translate (Vector3.up * player.GetComponent<Collider> ().bounds.size.y/2, Space.World);

		//Cast a ray from the player in front of them

		Debug.DrawRay (rayStart.transform.position, rayStart.transform.TransformDirection(Vector3.forward)*3, Color.blue, 3);

		RaycastHit hit;
		if(Physics.Raycast(rayStart.transform.position, rayStart.transform.TransformDirection(Vector3.forward), out hit, 3)) {
			if(hit.collider.gameObject.name.Contains("CrystalHolder (Interactable)")) {
				hit.collider.gameObject.name = "CrystalHolder";
				this.gameObject.GetComponent<CrystalRotator> ().enabled = true;
				Destroy(this.gameObject.GetComponent<Rigidbody> ());
				player.GetComponent<PlayerManager> ().inventory [0] = null;
				this.transform.SetParent (null);

				this.transform.SetPositionAndRotation (hit.collider.gameObject.transform.position, new Quaternion (0, 0, 0, 0));
				this.transform.Translate (Vector3.up * hit.collider.bounds.size.y / 3.5f);

				bool check = false;
				for (int i = 0; i <= 2; i++) {
					GameObject obj = GameObject.Find ("CrystalHolder (Interactable) (" + i + ")");
					if (obj != null)
						check = true;
				}

				if (!check) {
					//open door...?
					print ("The door can open");
					GameObject.Find ("Doorway").GetComponent<Doorway> ().activate ();
				}

			}
		}

		Destroy (rayStart);
		//Do a raycast forward from player
		//If it hits an obelisk gameobject, place this at the location of that obelisk and set trigger

	}

}
