using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour {

	public enum MonsterType {
		Prototype
	};

	public GameObject pathParent;
	public List<GameObject> pathNodes;
	public GameObject target;
	public GameObject rotateNode;
	public GameObject fromRot;

	public float monsterHealth;

	public float speed, turnSpeed, speedBase;
	public bool loopPath; //SET IN INSPECTOR ONLY
	public bool reverse;
	public bool countingDown;
	public int waitindex;
	public float[] wait, angle; //At <wait> frames, rotate to <angle> || //durations in Path wait[] array need to be in descending order, choosing the angle at time left (e.g., 10, 5, 2)
	public int currNode;

	public float timeleft;
	public float startTime;

	public float attackTimeLeft;
	public float attackTimeCooldown;
	public float baseAttackDistance, attackDistance, attackDistanceBuffer;

	public bool huntingPlayer;
	public float sawPlayerAt, sawPlayerDuration;

	public float reachDistance;
	public float visionDistance;
	public float rayCount;
	public float arcAngle;

	public abstract void init (); // This is used to initialise values that will vary from monster to monster
	public abstract void attackPlayer (); //This is how different monsters will attack

	// Use this for initialization
	void Start () {

		countingDown = false;

		init ();

		//Loads all the path nodes in the Parent Game Object
		for (int i = 0; i < pathParent.transform.childCount; i++) {
			pathNodes.Add (pathParent.transform.GetChild (i).gameObject);
		}

		for (int i = 0; i < pathParent.transform.childCount; i++) {
			if (i != 0 && i != (pathParent.transform.childCount-1)) {
				pathNodes [i].GetComponent<MonsterPath> ().prev = pathNodes [i - 1];
				pathNodes [i].GetComponent<MonsterPath> ().next = pathNodes [i + 1];
			} else if (i == 0) {
				pathNodes [i].GetComponent<MonsterPath> ().prev = pathNodes [pathParent.transform.childCount-1];
				pathNodes [i].GetComponent<MonsterPath> ().next = pathNodes [i + 1];
			} else if (i == pathParent.transform.childCount-1) { 
				pathNodes [i].GetComponent<MonsterPath> ().prev = pathNodes [i - 1];
				pathNodes [i].GetComponent<MonsterPath> ().next = pathNodes [0];
			}
		}

		target = pathNodes [0];
		rotateNode = this.transform.GetChild(0).gameObject;
		fromRot = this.transform.GetChild (1).gameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (!huntingPlayer) {
			if (timeleft > 0) {
				timeleft -= Time.deltaTime;
				float timeElapsed = startTime - timeleft;
				//print (timeElapsed);

				int max = -1;
				for (int i = 0; i < wait.Length; i++) {
				
					if (timeElapsed >= -wait [i]) {
						max = i;
					}
				}

				if (max != -1) {
					//set monster angle here
					rotateNode.transform.eulerAngles = new Vector3 (0, angle [max], 0);
					fromRot.transform.rotation = this.gameObject.transform.rotation;
				}

			} else if (timeleft < 0) {
				pathNodes [currNode].GetComponent<MonsterPath> ().fetchNextTarget ();
				countingDown = false;
				//moving = true;

			}
		} else {
			huntPlayer ();
		}

		//------------Movement------------------------
		if (target != null) {

			float step = speed * Time.deltaTime;
			float turn = turnSpeed * Time.deltaTime;
			this.transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);
		}

		//------------Facing and Rotation-------------
		//Ensure we are facing the next target node when moving between nodes
		if (target != null && countingDown != true || target != null && huntingPlayer) {
			Vector3 targetDirection = new Vector3(target.transform.position.x, rotateNode.transform.position.y, target.transform.position.z);
			rotateNode.transform.LookAt (targetDirection);
		}

		//Always be rotating main body towards rotateNode
		this.transform.rotation = Quaternion.Lerp (fromRot.transform.rotation, rotateNode.transform.rotation, turnSpeed);


		//-------------Raycasting----------------
		drawRays ();


	}

	public void waitAtAngleFor (int currNode, float[] wait, float[] angle) {
		this.currNode = currNode;
		this.wait = wait;
		this.angle = angle;

		startTime = Time.deltaTime;
		timeleft = 0;
		foreach (int i in wait) {
			timeleft += i;
		}
		countingDown = true;

	}

	public void setNextTarget(int currNode, GameObject prev, GameObject next) {

		if (!loopPath && currNode == pathNodes.Count - 1) {
			reverse = true;
		}
		if (!loopPath && currNode == 0) {
			reverse = false;
		}

		if (reverse)
			target = prev;
		else
			target = next;

	}

	public void drawRays() {

		bool seenPlayer = false;
		for (int i = 0; i < rayCount; i++) {
			//Vector3 shootVec = rotateNode.transform.rotation * Quaternion.AngleAxis (arcAngle * (i / rayCount) - arcAngle / 2, Vector3.up) * Vector3.forward*10;
			Vector3 shootVec = transform.rotation * Quaternion.AngleAxis ((arcAngle * (i / rayCount)) - (arcAngle / 2), Vector3.up) * Vector3.forward*visionDistance;

			RaycastHit hit;
			if (Physics.Raycast (transform.position, shootVec, out hit, visionDistance)) {
				//print (hit.collider.gameObject);
				if (hit.collider.gameObject.name == "playerCharacter") {
					attackDistanceBuffer = hit.collider.bounds.size.x;
					seenPlayer = true;
					Debug.DrawLine (rotateNode.transform.position, hit.point, Color.red);

					if (!huntingPlayer) {
						target = hit.collider.gameObject;
						sawPlayerAt = Time.time;
						huntingPlayer = true;
					}
				}
			}
			Debug.DrawRay(transform.position, shootVec, Color.green, .1f);
		}

//		Debug.DrawRay (transform.position, -(new Vector3(0,0,0) - transform.TransformDirection(Vector3.forward)*15), Color.red, 0.01f);
	}

	public void huntPlayer() {
		sawPlayerDuration = (Time.time - sawPlayerAt);
		float distance = Vector3.Distance (this.transform.position, target.transform.position);

		if (attackTimeLeft > 0) {
			attackTimeLeft -= Time.deltaTime;
		}

		if (sawPlayerDuration < 1) { //Wait for one second before chasing player
			speed = 0;
		} else {
			if (distance < (attackDistance)) { //Stopped and Attacking Player
				speed = 0.0f;
				this.gameObject.GetComponent<MeshRenderer> ().material = Resources.Load ("Models/Materials/blue") as Material;
				if (attackTimeLeft <= 0) {
					attackPlayer ();// - Probably play an animation here with an event that creates a hitbox or something
					attackTimeLeft = attackTimeCooldown;
				}
			} else if (distance > (reachDistance-attackDistanceBuffer)) { //Chasing Player
				speed = speedBase * 2;
				this.gameObject.GetComponent<MeshRenderer> ().material = Resources.Load ("Models/Materials/green") as Material;
			}
		}
	}

	public void takeDamage(float damage) {
		monsterHealth -= damage;

		if (monsterHealth <= 0) {
			commitDie ();
		}
	}

	public void commitDie() {
		Destroy (this.gameObject);
	}
}
