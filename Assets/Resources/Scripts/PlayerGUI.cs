using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour {

	// Use this for initialization
	public GameObject cameraTarget;
	public GameObject rootCanvas;
	public GameObject inventoryImage;
	public GameObject[] item;

	public GameObject player; 
	public PlayerManager playerManager;

	public Sprite[] imageLibrary;

	void Awake() {
		
		print (GameObject.Find("CameraTarget"));
		cameraTarget = GameObject.Find ("CameraTarget");
		rootCanvas = cameraTarget.transform.Find("Camera/Canvas").gameObject;
		inventoryImage = rootCanvas.transform.Find("Canvas/Inventory").gameObject;

		player = this.gameObject;
		playerManager = player.GetComponent<PlayerManager> ();

		item = new GameObject[3];
		item[0] = inventoryImage.transform.Find ("1/Image").gameObject;
		item[1] = inventoryImage.transform.Find ("2/Image").gameObject;
		item[2] = inventoryImage.transform.Find ("3/Image").gameObject;

		imageLibrary = new Sprite[4];
		imageLibrary [0] = Resources.Load<Sprite> ("images/smokey") as Sprite;
		imageLibrary [1] = Resources.Load<Sprite> ("images/Rock") as Sprite;
		imageLibrary [2] = Resources.Load<Sprite> ("images/Torch") as Sprite;
		imageLibrary [3] = Resources.Load<Sprite> ("images/Blade") as Sprite;
	}

	void Update() {
		//Ping to see what item is in each inventory slot
		//Change image depending on what item is in there

		//print ("--------------");
		if (player!=null)
		for (int i = 0; i < 3; i++) {
			if (playerManager.inventory[i] != null) {
				if (playerManager.inventory[i].name.Contains ("torch")) {
					item [i].GetComponent<Image> ().sprite = imageLibrary [2];
				} else if (playerManager.inventory[i].name.Contains ("rock")) {
					item [i].GetComponent<Image> ().sprite = imageLibrary [1];
					} else if (playerManager.inventory[i].name.Contains ("Empty")) {
						item [i].GetComponent<Image> ().sprite = imageLibrary [0];
					}
			} else {
				item [i].GetComponent<Image> ().sprite = imageLibrary [0];
			}
		}
	}
}
