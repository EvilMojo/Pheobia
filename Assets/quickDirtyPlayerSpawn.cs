using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickDirtyPlayerSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("playerCharacter").transform.SetPositionAndRotation (this.transform.position, this.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
