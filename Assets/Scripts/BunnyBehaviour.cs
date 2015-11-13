using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BunnyBehaviour : MonoBehaviour {

	public GameObject carrot;
	public GameObject escapeHole;	
	public GameObject parentBunny;	//another bunny that this looks at
	public float walkSpeed = 25;
	public float runSpeed = 100;
	public float eatTime = 2;
	public float jumpStrength = 20;
	public bool noParent;
	public string type;
	
	private Animator bunnyAnimator;
	private BunnyController bc;
	private string[] jumpColliders;
	private float startTime;
	public bool flee;
	private bool eating;
	private bool returning;
	private bool finished;

	private Rigidbody rb;

	//!!!! need to check if carrot is null before assigning a new carrot to the bunny

	void Start () {
		flee = false;
		eating = false;
		returning = false;
		finished = false;
		startTime = Time.time;
		jumpColliders = new string[5]{"Enemy", "Stage", "EnemyPart", "Destructible", "Player"};
		bunnyAnimator = this.GetComponent<Animator> ();
		bc = ExtraFunctions.searchInAncestor (transform, typeof(BunnyController)).GetComponent<BunnyController>();
		rb = GetComponent<Rigidbody> ();
	}

	public string getBunnyType(){
		return type;
	}

	public bool getFinished(){
		return finished;
	}

	void OnCollisionEnter(Collision c){
		if (carrot != null && c.collider.name == carrot.name && !eating) {
			eating = true;
			startTime = Time.time;
		} else if (returning && c.collider.name == escapeHole.name) {
			returned();
		}

		if (!eating && Array.IndexOf (jumpColliders, c.collider.tag) >= 0){
			//print (gameObject.name + " collided with " + c.collider.name);
			if (rb.velocity.y <= 0){
				rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
			}
		}
	}

	void returned(){
		finished = true;
		this.gameObject.SetActive(false);
	}

	void moveTo(GameObject g){
		bunnyAnimator.SetBool ("Moving", true);
		Vector3 desiredForward = (g.transform.position - transform.position).normalized;
		desiredForward.y = 0;
		transform.forward = desiredForward;
		if (!flee) {
			rb.velocity = new Vector3 (transform.forward.x * walkSpeed, 
			                           this.rb.velocity.y, 
			                           transform.forward.z * walkSpeed);
			//transform.position = Vector3.Lerp (transform.position, g.transform.position, Time.deltaTime * walkSpeed / 2);
		} else {
			rb.velocity = new Vector3 (transform.forward.x * runSpeed, 
			                           this.rb.velocity.y, 
			                           transform.forward.z * runSpeed);

			//transform.position = Vector3.Lerp (transform.position, g.transform.position, Time.deltaTime * runSpeed / 2);
		}
		transform.Rotate (new Vector3 (0, -90, 0));

		bool inRange = (transform.position.x >= escapeHole.transform.position.x - escapeHole.transform.localScale.x / 2
			&& transform.position.x <= escapeHole.transform.position.x + escapeHole.transform.localScale.x / 2) 
			&& (transform.position.z >= escapeHole.transform.position.z - escapeHole.transform.localScale.y / 2
			&& transform.position.z <= escapeHole.transform.position.z + escapeHole.transform.localScale.y / 2);

		if (inRange) {
			returned();
		}
		/*
		float roundedThisX = Mathf.Round (transform.position.x * 10) / 10;
		float roundedThisZ = Mathf.Round (transform.position.z * 10) / 10;
		float roundedHoleX = Mathf.Round (escapeHole.transform.position.x * 10) / 10;
		float roundedHoleZ = Mathf.Round (escapeHole.transform.position.z * 10) / 10;
		if (roundedThisX == roundedHoleX && roundedHoleZ == roundedThisZ) {
			returned();
		}
		*/


	}

	public bool getFlee(){
		return flee;
	}

	void checkParentBunny (){
		if (parentBunny == null) {
			if (!noParent){
				flee = true;
				bc.allFlee();
			}
		} else {
			BunnyBehaviour bnb = parentBunny.GetComponent<BunnyBehaviour>();
			flee = bnb.getFlee();
		}
		if (flee) {
			bunnyAnimator.SetBool ("Running", true);
		} else {
			bunnyAnimator.SetBool("Running", false);
		}
	}
	


	void Update () {
		checkParentBunny ();

		if (eating && Time.time - startTime > eatTime) {
			if (!flee && carrot != null){
				//Destroy(carrot.gameObject);
				carrot = bc.getNewCarrot(carrot.gameObject);
				eating = false;
			}else{
				eating = false;
			}
		}
		if (!flee) {
			if (carrot != null) {
				if (!eating) {
					moveTo (carrot);
				}else{
					bunnyAnimator.SetBool("Moving", false);
				}
			} else {
				carrot = bc.getNewCarrot();
				returning = true;
				moveTo (escapeHole);
			}
		} else {
			returning = true;
			moveTo (escapeHole);
		}
	}
}
