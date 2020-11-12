using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item : Interactable {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("playerCharacter");
	}

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
	}

	public virtual void useItem() {
		print ("USE ITEM: " + this.name);
	}

	public void pickupItem() {
		
		Destroy (this.gameObject.GetComponent<Rigidbody> ());
		this.gameObject.GetComponent<Collider> ().enabled = false;

		player.GetComponent<PlayerManager> ().inventory[0] = this.gameObject;

		moveItem ();
	
	}

	public void moveItem() {
		this.gameObject.transform.position = player.transform.Find ("playerMesh/Armature/Root/Torso/Chest/Neck/Shoulder_L/Bicep_L/Forearm_L/Hand_L").transform.position;
		this.gameObject.transform.rotation = player.transform.Find ("playerMesh/Armature/Root/Torso/Chest/Neck/Shoulder_L/Bicep_L/Forearm_L/Hand_L").transform.rotation;
		this.gameObject.transform.SetParent (player.transform.Find ("playerMesh/Armature/Root/Torso/Chest/Neck/Shoulder_L/Bicep_L/Forearm_L/Hand_L").transform);
	}

	public void dropItem() {
		this.gameObject.AddComponent<Rigidbody>();
		this.gameObject.GetComponent<Collider> ().enabled = true;

		//player.GetComponent<PlayerManager> ().inventory[0] = new GameObject();
		//player.GetComponent<PlayerManager> ().inventory[0].name = "Empty";

		player.GetComponent<PlayerManager> ().inventory [0] = null;


		this.gameObject.transform.SetParent (null);
		SceneManager.MoveGameObjectToScene (this.gameObject, SceneManager.GetActiveScene ());
	}

	public void throwItem() {
		if (player == null)
			player = GameObject.Find ("playerCharacter");
		this.transform.rotation = player.transform.rotation;
		dropItem ();
		this.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 750);
		this.gameObject.GetComponent<Rigidbody>().AddTorque(transform.right * 100);
	}
}
