using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactable {
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void executeInteraction (){ 
		//Light the fire
		GameObject.Find("playerCharacter").GetComponent<PlayerManager>().resetSpawnPoint();
	}

	public void setSpawnPoint(GameObject spawnPoint) {
		Destroy (spawnPoint);
		//print ("SETTING SPAWNPOINT");
		//print (spawnPoint);
		//spawnPoint.transform.SetParent(this.gameObject.transform);

	}

}