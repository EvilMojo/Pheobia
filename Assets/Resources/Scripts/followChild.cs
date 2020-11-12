using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followChild : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = new Vector3 (this.transform.GetChild (0).transform.position.x, this.transform.GetChild (0).transform.position.y, this.transform.GetChild (0).transform.position.z);
	}
}
