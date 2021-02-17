using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {

	public GameObject levelManager;

    public GameObject player;
    public List<GameObject> monsters;
	public List<GameObject> traps;
	public List<GameObject> interactables;
	public List<GameObject> items;
	public List<GameObject> genericData;

	public string loadedFlag;

	public Vector3 playerSpawnLocation;

	// Use this for initialization
	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");

        monsters = new List<GameObject>();
        traps = new List<GameObject>();
        interactables = new List<GameObject>();
        items = new List<GameObject>();
        genericData = new List<GameObject>();

        createData();
        loadData();
        cleanData();
        setActivated();
    }

    public void createData() {
		levelManager = GameObject.Find ("LevelManager");
//		this.transform.SetParent (levelManager.transform);
		//loadedFlag = levelManager.GetComponent<LevelManager> ().loadedFlag;


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
        //if(GameObject.FindGameObjectsWithTag("GenericLevelData")!=null)
		foreach (GameObject generic in GameObject.FindGameObjectsWithTag("GenericLevelData")){
            //Not as simple as this, we need to take the information, not the object itself
            //genericData.Add (generic);

            LevelManager.State newObj;
            newObj.name = generic.name;
            newObj.rotation = generic.transform.rotation;
            newObj.position = generic.transform.position;
            newObj.activated = false;

        }
	}

	public void saveData() {
		//levelManager.GetComponent<LevelManager> ().saveLevelObjects (genericData);

		print ("Saving data...");

		foreach (GameObject obj in genericData)
		{
			if (obj != null) {
				LevelManager.State data;

				data.name = obj.name;
				data.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
				data.rotation = new Quaternion(0, 0, 0, 0);
				data.activated = false;

				if (obj.GetComponent<Activatable>() != null) {
					print("Activatable");
					data.activated = obj.GetComponent<Activatable>().activated;
				}

				//genericDataList.Add(data);

				print("Saved: " + data.name + " [" + data.position + "]");
			} else {
				print("Obj null");
			}
		}
	}

	public void loadData() {
	//	levelManager.GetComponent<LevelManager> ().loadLevelObjects (interactables);

		foreach (GameObject data in genericData) {
			GameObject obj = GameObject.Find (data.name);
			if (obj != null) {
				print ("Loading " + data.name + " data");
				//obj.name = obj.name + loadedFlag; // this is assuming the  LevelData loads all objects in the same, sequential order
				obj.transform.position = new Vector3 (data.transform.position.x, data.transform.position.y, data.transform.position.z);
			}
		}
	}

	public void cleanData() {
		if (interactables!=null)
		foreach (GameObject obj in genericData) {
			if (obj.name.Contains("_")) {
				string[] splits = obj.name.Split ('_');
				print (splits [0] + "|" + splits [1]);
				obj.name = splits [0].Trim();
			}
		}
	}

	public void setActivated() {
		if (genericData != null)
		foreach (GameObject obj in genericData) {
			//print (obj.name);
			if (obj.GetComponent<Activatable>() != null && obj.GetComponent<Activatable> ().activated == true) {
				print (obj.name + " is activatable");
				obj.GetComponent<Activatable> ().activate ();
			}
		}
	}


}
