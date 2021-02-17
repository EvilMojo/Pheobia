using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	public float health;
	public float MAX_HEALTH;
	public bool dead;
	public float deadzone;
	public GameObject copy;
	public GameObject[] inventory, spawnInventory;
	public const int inventorySize = 3; //0 = Empty (Always) //Not yet

	//Camera Controls
	public GameObject cameraTarget;

	//Player Keyboard Inputs
	//Will need to use code below to convert string to keycode
	//KeyCode thisKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Whatever") ;
	public KeyCode up, down, left, right; //movement
	public KeyCode interact, useItem, climb, unused; //xbox A,B,X,Y buttons
	public KeyCode run, throwItem; //xbox triggers
	public KeyCode cycleLeft, cycleRight; //xbox bumpers

	//External Player Scripts
	public PlayerInteraction interactionScript;
	public PlayerMovement movementScript;
	public PlayerLook lookScript;
	public PlayerGUI guiScript;

	// Use this for initialization
	void Start () {

		setPlayerInputs ();

		dead = false;

		//Get Camera
		cameraTarget = GameObject.Find("CameraTarget");

		//Get player scripts
		interactionScript = this.gameObject.GetComponent<PlayerInteraction>();
		movementScript = this.gameObject.GetComponent<PlayerMovement>();
		lookScript = this.gameObject.transform.Find("LookCentre").gameObject.GetComponent<PlayerLook>();
		guiScript = this.gameObject.GetComponent<PlayerGUI> ();

		inventory = new GameObject[inventorySize];
		spawnInventory = new GameObject[inventorySize];

		deadzone = 0.25f;
		health = MAX_HEALTH = 1.0f;
		//spawnPoint = this.transform.Find ("PlayerSpawnPoint").gameObject;

		/*cameraTarget = GameObject.Find ("CameraTarget");
		rootCanvas = cameraTarget.transform.Find("Camera/Canvas").gameObject;
		inventoryImage = rootCanvas.transform.Find("Canvas/Inventory").gameObject;

		itemImage = new GameObject[3];
		itemImage[0] = inventoryImage.transform.Find ("1/Image").gameObject;
		itemImage[1] = inventoryImage.transform.Find ("2/Image").gameObject;
		itemImage[2] = inventoryImage.transform.Find ("3/Image").gameObject;
*/

	}

	public void setPlayerInputs() {
		up = (KeyCode) System.Enum.Parse(typeof(KeyCode), "W");
		down = (KeyCode) System.Enum.Parse(typeof(KeyCode), "S");
		left = (KeyCode) System.Enum.Parse(typeof(KeyCode), "A");
		right = (KeyCode) System.Enum.Parse(typeof(KeyCode), "D");
		interact = (KeyCode) System.Enum.Parse(typeof(KeyCode), "E");
		useItem = (KeyCode) System.Enum.Parse(typeof(KeyCode), "F");
		climb = (KeyCode) System.Enum.Parse(typeof(KeyCode), "Q");
		run = KeyCode.LeftShift;
		throwItem = (KeyCode) System.Enum.Parse(typeof(KeyCode), "R");
		cycleRight = KeyCode.Tab;
	}

	public void preserve() {
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	//Update should handle player inputs and parse to the game. No other scripts should handle player input
	//Inputs currently exist in and need to be removed from:
	//Climber
	void Update () {

		//print (this.transform.rotation.eulerAngles.y);
        /*
		if (!dead) {
			cameraTarget.transform.Find("Fader").gameObject.GetComponent<Fader>().faded = false;
			this.gameObject.SetActive (true);
			//Destroy (GameObject.Find("playerCharacter ragdoll"));
		}*/

		if (GameObject.Find ("PlayerSpawnPoint") == null) {
			print ("CREATING NEW SPAWNPOINT");
			GameObject spawn = new GameObject ();
			spawn.name = "PlayerSpawnPoint";
			spawn.transform.SetPositionAndRotation (this.gameObject.transform.position, this.gameObject.transform.rotation);
		}

		//Non-Movement Input Management
			
		//Used to switch what item is being highlighted in inventory
		if (Input.GetButtonDown ("IncrementInventory") || Input.GetKeyDown(cycleRight)) {
			cycleItem (true);
		} else if (Input.GetButtonDown ("DecrementInventory") || Input.GetKeyDown(cycleLeft)) {
			cycleItem (false);
		}

		//A - Interact with environment objects
		if (Input.GetButtonDown("Interact") || Input.GetKeyDown(interact)) { //Xbox 360 'A' button
			if (interactionScript.interact () == false) {
                if (inventory [0] != null)
				inventory [0].GetComponent<Item> ().dropItem ();
			}
		}

		//B - Use the item the player is holding
		if (Input.GetButtonDown("UseItem")  || Input.GetKeyDown(useItem)) { //Xbox 360 'B' button
			if (this.gameObject.GetComponent<Climber> ().hanging) {
				this.gameObject.GetComponent<Climber> ().drop ();
			} else {
				if (inventory [0] != null) {
					inventory [0].GetComponent<Item> ().useItem ();
				} else {
					Debug.Log ("NO ITEM IN HAND");
				}
			}
		}
	
		//X - Climb stuff
		if (Input.GetButtonDown("Climb") || Input.GetKeyDown(climb)) {//Xbox 360 'X' button
			//this.gameObject.GetComponent<Climber> ().climb ();
			if (this.gameObject.GetComponent<Climber> ().hanging == false) {
				this.gameObject.GetComponent<Climber> ().processClimb (this.gameObject.GetComponent<Climber> ().drawClimbingRay (this.gameObject.transform), this.gameObject);
			} else {
				this.gameObject.GetComponent<Climber> ().processClimb (this.gameObject.GetComponent<Climber> ().drawClimbingRay (this.gameObject.transform), this.gameObject);
			}
		}

		//Y - Used to access inventory item
		if (Input.GetButtonDown("Inventory") || Input.GetKeyDown(unused)) {//Xbox 360 'Y' button
			//if (itemInHand != null) {
			//swapItem ();
			//} else {
			//	retrieveHighlightedItem ();
			//}
		}

		if (Input.GetAxis("PlayerLeftTrigger") > deadzone || Input.GetKey(run)) {
			this.GetComponent<PlayerMovement>().moveSpeed = 6.5f;
		} else {
			this.GetComponent<PlayerMovement>().moveSpeed = 3.0f;
		}

		if (Input.GetAxis("PlayerRightTrigger") > deadzone || Input.GetKeyDown(throwItem)) {
			if (inventory [0] != null) {
				inventory [0].GetComponent<Item> ().throwItem ();
			}
		}
	}

	public void commitDie() {

		//this.transform.position = new Vector3 (spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
		//this.transform.rotation = new Quaternion (spawnPoint.transform.rotation.x, spawnPoint.transform.rotation.y, spawnPoint.transform.rotation.z, spawnPoint.transform.rotation.w);
		//this.gameObject.transform.Find("playerMesh").GetComponent<MeshRenderer> ().material.color = Color.red;
		//health = MAX_HEALTH;
		if (inventory [0] != null) {
			inventory [0].GetComponent<Item> ().throwItem ();
		}
		this.gameObject.GetComponent<PlayerMovement> ().enabled = false;


		GameObject ragdoll = Instantiate (this.gameObject, transform.position, transform.rotation);
		ragdoll = Instantiate (this.gameObject, transform.position, transform.rotation);
		ragdoll.name = "playerCharacter ragdoll";
		ragdoll.GetComponent<RagDoll> ().activateRagDoll (true);
		cameraTarget.transform.Find ("Fader").gameObject.GetComponent<Fader> ().toggleFadeAfter (3.0f, true);

		this.gameObject.SetActive (false);
		dead = true;
	}

	public void commitDie(Vector3 direction, float force) {

		//GameObject newPlayer 

		//this.transform.position = new Vector3 (spawnPoint.transform.position.x, spawnPoint.transform.position.y, spawnPoint.transform.position.z);
		//this.transform.rotation = new Quaternion (spawnPoint.transform.rotation.x, spawnPoint.transform.rotation.y, spawnPoint.transform.rotation.z, spawnPoint.transform.rotation.w);
		//this.gameObject.transform.Find("playerMesh").GetComponent<MeshRenderer> ().material.color = Color.red;
		//health = MAX_HEALTH;


		if (inventory [0] != null) {
			inventory [0].GetComponent<Item> ().throwItem ();
		}
		this.gameObject.GetComponent<PlayerMovement> ().enabled = false;

		copy = Instantiate (this.gameObject, transform.position, transform.rotation);
		copy.GetComponent<PlayerMovement> ().enabled = false;
		DontDestroyOnLoad (copy);
		copy.name = "playerCopy";
		copy.SetActive (false);
		dead = true;

		this.gameObject.GetComponent<RagDoll> ().activateRagDoll (true);
		this.gameObject.GetComponent<RagDoll> ().applyForceToRagDoll (direction, force);

		cameraTarget.transform.Find ("Fader").gameObject.GetComponent<Fader> ().toggleFadeAfter (3.0f, true);

	}

	public void revive() {
		if (dead) {
			
			copy.SetActive (true);
			this.name = "dead";
			copy.name = "playerCharacter";

			cameraTarget.GetComponent<CameraControl> ().player = copy;
			cameraTarget.GetComponent<CameraControl> ().target = copy;
			GameObject spawnPoint = GameObject.Find ("PlayerSpawnPoint");
			copy.transform.SetPositionAndRotation (spawnPoint.transform.position, spawnPoint.transform.rotation);

			for (int i = 0; i < 3; i++) {
				this.inventory [i] = null;
			}

			for (int i = 0; i < 3; i++) {
				print (i + ": " + this.spawnInventory [i]);
				if (this.spawnInventory[i] != null) {
					copy.GetComponent<PlayerManager> ().inventory [i] = this.spawnInventory [i];
					copy.GetComponent<PlayerManager> ().inventory [i].SetActive (true);
					copy.GetComponent<PlayerManager> ().inventory [i].GetComponent<Item> ().moveItem ();
					copy.GetComponent<PlayerManager> ().inventory [i].transform.SetParent (copy.transform);
					copy.GetComponent<PlayerManager> ().inventory [i].SetActive (false);
				}
			}

			Destroy (this.gameObject);
			dead = false;

			copy.GetComponent<PlayerMovement> ().enabled = true;

			cameraTarget.transform.Find ("Fader").gameObject.GetComponent<Fader> ().respawnOk = true;
		}
	}

	public void resetSpawnPoint() {

		Destroy (GameObject.Find ("PlayerSpawnPoint"));
		for (int i = 0; i < 3; i++) {
			spawnInventory [i] = inventory [i];
		}
		/*spawnPoint.transform.SetParent (null);
		spawnPoint.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
		spawnPoint.transform.rotation = new Quaternion (this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
		spawnPoint.transform.SetParent (this.gameObject.transform);*/
	}

	public void cycleItem(bool increment) {
	
		GameObject swap;
		GameObject empty = new GameObject();
		empty.name = "Empty";

		for (int i = 0; i<3; i++) {
			if (inventory [i] == null) {
				inventory [i] = empty;
			}
		}

		if (increment) {
			swap = inventory [0];
			inventory [0] = inventory [1];
			inventory [1] = inventory [2];
			inventory [2] = swap;
		} else {
			swap = inventory [0];
			inventory [0] = inventory [2];
			inventory [2] = inventory [1];
			inventory [1] = swap;
		}

		inventory [2].SetActive (false);
		inventory [1].SetActive (false);
		inventory [0].SetActive (true);

		Destroy (empty);

		/*
		/*if (inventory [inventoryHighlight] == null) {
			inventory [inventoryHighlight] = item;
			itemInHand = null;
		} else {
		GameObject swap;
		swap = inventory [inventoryHighlight];
		inventory [inventoryHighlight] = itemInHand;
		itemInHand = swap;
		itemInHand.SetActive (true);
		inventory [inventoryHighlight].SetActive (false);
		//Destroy (swap);
		//}
		*/

	}
}