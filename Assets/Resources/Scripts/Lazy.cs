using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lazy : MonoBehaviour {
	
	void Awake () {
		if (GameObject.Find("GameManager") == null) {
			print ("Game Manager Not Found");
			SceneManager.LoadSceneAsync ("DataHold");
		}
	}
}
