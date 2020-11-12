using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

	public GameObject levelManager;

	public List<GameObject> monsters;
	public GameObject player;
	public List<GameObject> traps;
	public List<GameObject> interactables;
	public List<GameObject> items;
	public List<GameObject> genericData;

	public string loadedFlag;

	// Use this for initialization
	void Start () {
		createData ();
		loadData ();
		cleanData ();
		setActivated ();
	}

	public void createData() {
		levelManager = GameObject.Find ("LevelManager");
//		this.transform.SetParent (levelManager.transform);
		//loadedFlag = levelManager.GetComponent<LevelManager> ().loadedFlag;

		player = GameObject.FindGameObjectWithTag("Player");

		//Get the monsters
		foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster")) {
			//monsters.Add (monster);
		}

		//Get the traps
		foreach (GameObject trap in GameObject.FindGameObjectsWithTag("Trap")){
			//traps.Add (trap);
		}
		//Get the interactables
		foreach (GameObject interactable in GameObject.FindGameObjectsWithTag("Interactable")){
			//interactables.Add (interactable);
		}

		//Get the items
		foreach (GameObject item in GameObject.FindGameObjectsWithTag("Item")){
			//items.Add (item);
		}

		//Get anything we want to save but doesn't fall into above categories (probably just for location/rotation data)
		foreach (GameObject generic in GameObject.FindGameObjectsWithTag("GenericLevelData")){
			//genericData.Add (generic);
		}
	}

	public void saveData() {
		levelManager.GetComponent<LevelManager> ().saveLevelObjects (interactables);
	}

	public void loadData() {
	//	levelManager.GetComponent<LevelManager> ().loadLevelObjects (interactables);
	}

	public void cleanData() {
		if (interactables!=null)
		foreach (GameObject obj in interactables) {
			if (obj.name.Contains("_")) {
				string[] splits = obj.name.Split ('_');
				print (splits [0] + "|" + splits [1]);
				obj.name = splits [0].Trim();
			}
		}
	}

	public void setActivated() {
		if (interactables != null)
		foreach (GameObject obj in interactables) {
			//print (obj.name);
			if (obj.GetComponent<Activatable>() != null && obj.GetComponent<Activatable> ().activated == true) {
				print (obj.name + " is activatable");
				obj.GetComponent<Activatable> ().activate ();
			}
		}
	}


}
