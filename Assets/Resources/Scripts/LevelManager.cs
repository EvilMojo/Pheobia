using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public List<GameObject> levelData;

	public struct State {
		public string name;
		public Vector3 position;
		public Quaternion rotation;
		public bool activated;
	}


	public List<State> interactablesData; 

	public string loadedFlag = "_[Loaded]";

	/* A level manager will be responsible for controlling the state of all objects within a level
	 * 
	 * Example, should the player die, the level manager will need to
	 * 	- Reset the player in their original position
	 *  - Keep track of all monsters and their initial state
	 *  - Keep track of all traps and their inital state
	 *  - Keep track of all interactable objects and their initial state
	 *  - Keep track of all inventory item objects and their initial state
	 *  - Revert all tracked objects to initial state on player death
	 *       - OR - 
	 *  - Track new initial state when player saves or traverses a level
	 */

	void Awake () {
		//Get the player object
		//DontDestroyOnLoad (this.gameObject);
		//generateLevel ("Tutorial");

		levelData = new List<GameObject> ();
	}

	public void generateLevel(string levelName) {
		SceneManager.LoadSceneAsync (levelName);
		if (findLevelData(levelName)) {
			loadLevel (levelName);
		} else {
			createLevel(levelName);
		}
	}

	public void createLevel(string levelName) {

		//Only for if we need to create a new object. But this doesn't add data from tags
		GameObject newLevelData = new GameObject(levelName + " Level Data");
		newLevelData.transform.SetParent (this.gameObject.transform);
		levelData.Add (newLevelData);

		newLevelData.AddComponent<LevelData>();
		newLevelData.GetComponent<LevelData>().createData ();

		foreach (GameObject obj in levelData) {
			print (obj.name);
		}
	}

	public bool findLevelData(string levelName) {
		foreach (GameObject level in levelData) {
			if (level.name == (levelName + " Level Data")) {
				return true;
			}
		}
		return false;
	}

	public void loadLevel(string levelName) {
		
	}

	public void saveLevelObjects(List<GameObject> interactables) {

		print ("Saving data...");

		interactablesData = new List<State> ();
		foreach (GameObject obj in interactables) {
			State data;

			data.name = obj.name;
			data.position = new Vector3 (obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
			data.rotation = new Quaternion (0, 0, 0, 0);
			data.activated = false;

			if (obj.GetComponent<Activatable> () != null) {
				print ("Activatable");
				data.activated = obj.GetComponent<Activatable>().activated;
			}

			interactablesData.Add (data);

			print ("Saved: " + data.name + " [" + data.position + "]");
		}
	}

	public void loadLevelObjects(List<GameObject> interactables) {
		if (interactablesData == null) {
		} else if (interactablesData.Count == 0) {
		} else if (interactablesData.Count > 0) {
			foreach (GameObject obj in interactables) {
				foreach (State data in interactablesData) {
					if (obj.name == data.name) {
						print ("Loading " + data.name + " data");
						obj.name = obj.name + loadedFlag; // this is assuming the  LevelData loads all objects in the same, sequential order
						obj.transform.position = new Vector3 (data.position.x, data.position.y, data.position.z);

						if (obj.GetComponent<Activatable> () != null) {
							print ("Activatable");
							obj.GetComponent<Activatable> ().activated = data.activated;
						}
					}
				}
			}
		}
	}
}