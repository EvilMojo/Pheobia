using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrototype : Monster {

	public float attackRayCount;

	public override void init() {

		monsterHealth = 1;
		this.name = "Prototype";
		attackDistance = 4.0f;
		reachDistance = 6.0f;
		visionDistance = 20;
		rayCount = 30;
		arcAngle = 120;
		speedBase = 10.0f;
		speed = speedBase;
		turnSpeed = 0.1f;
		attackTimeCooldown = 5.0f;
		baseAttackDistance = attackDistance = 5.0f;

		//Ranged Prototype
		//Need to change base variables to create distance etc

		//baseAttackDistance = attackDistance = 50.0f;
		//reachDistance = 51.0f;
		//visionDistance = 60.0f;

	}

	public override void attackPlayer() {

		//GameObject hitBox = new GameObject(this.name + " attack");

		//hitBox.AddComponent<SelfDestructTimer> ();

		//hitBox.GetComponent<MonsterAttack> ().init(this.gameObject);

		this.gameObject.GetComponent<MeshRenderer> ().material = Resources.Load ("Models/Materials/red") as Material;

		//Four methods to do this

		//1. Raycasting: Use rays at a short (or long) distance to check collision with the player
		//2. Hitboxes: Create a hitbox that spawns in and despawns after a small amount of time
		//3. Active/Deactive: Have the monster with permanently attached attack spheres. Activate spheres when attacking, deactivate when idle
		//4. GameObject Collision: Create a gameobject (ideal for long range attacks) with velocity towards player

		//1. Raycasting



		 for (int i = 0; i < rayCount; i++) {
			Vector3 shootVec = transform.rotation * Quaternion.AngleAxis ((arcAngle * (i / rayCount)) - (arcAngle / 2), Vector3.up) * Vector3.forward*(attackDistance + attackDistanceBuffer);

			RaycastHit hit;
			if (Physics.Raycast (transform.position, shootVec, out hit, attackDistance + attackDistanceBuffer)) {
				print (hit.collider.gameObject.name);
				if (hit.collider.gameObject.name == "playerCharacter") {
					//Debug.DrawLine (rotateNode.transform.position, hit.point, Color.red);

					Vector3 forceVector = new Vector3 (hit.collider.gameObject.transform.position.x - this.transform.position.x, 
						0, 
						hit.collider.gameObject.transform.position.z - this.transform.position.z);

					hit.collider.gameObject.GetComponent<PlayerManager>().commitDie(forceVector, 5.0f);
					//hit.collider.transform.Find("playerMesh").Find("bodyMesh").gameObject.GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
				}
			}
			Debug.DrawRay(transform.position, shootVec, Color.red, 20);
		}



		//2. Hitboxes:

		/*

		GameObject attackBox = new GameObject();

		attackBox.transform.position = this.gameObject.transform.position;

		attackBox.transform.Translate (this.gameObject.transform.forward*attackDistance);

		attackBox.AddComponent<SphereCollider> ();
		//attackBox.GetComponent<SphereCollider> ().bounds.center = attackBox.transform.position;
		attackBox.GetComponent<SphereCollider> ().radius = 2.0f;
		attackBox.GetComponent<SphereCollider> ().isTrigger = true;

		attackBox.AddComponent<MonsterAttack> ();

		attackBox.AddComponent<SelfDestructTimer> ();
		attackBox.GetComponent<SelfDestructTimer>().setTimer(1.0f);

*/

		//3. Active and Deactive
		//For the protoype the hitbox will not move, but on characters it will be parented to hands or something so they move when active



		//4. GameObject Collision (Projectile)
		//For the prototype, this will be instant. Delays and other details may exist depending on Monster
		/*
		 * 
		//Create spear
		GameObject projectile = Instantiate (Resources.Load ("Prefabs/spear(thrown)")) as GameObject;

		projectile.AddComponent<MonsterSpearAttack> ();

		//Set spear to location of monster hand (Prototype: Body)
		projectile.GetComponent<MonsterSpearAttack> ().setPosition(this.gameObject);

		//Set spear angle towards target (usually player)
		projectile.GetComponent<MonsterSpearAttack> ().faceTarget(target);

		//parent spear to monster hand (Prototype: None)

		//launch spear in direction of player
		projectile.GetComponent<MonsterSpearAttack> ().launch(this.gameObject);

		projectile.AddComponent<SelfDestructTimer> ();
		projectile.GetComponent<SelfDestructTimer> ().setTimer (2.0f);
		*/
	}
}