using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.name == "playerMesh") {
			other.gameObject.GetComponent<MeshRenderer> ().material.color = Color.red;
		}
	}

	public void setPosition(GameObject owningObject) {

		this.gameObject.transform.position = owningObject.transform.position;
		this.gameObject.transform.Translate (0, owningObject.GetComponent<MeshFilter>().mesh.bounds.size.y/2, 10);
	}

	public void faceTarget(GameObject target) {

		Vector3 throwDirection = new Vector3(target.transform.position.x, target.GetComponent<MeshFilter>().mesh.bounds.size.y/2, target.transform.position.z);
		//projectile.transform.eulerAngles = throwDirection;
		this.gameObject.transform.LookAt (throwDirection);
	}
}
