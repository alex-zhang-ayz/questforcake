using UnityEngine;
using System.Collections;
using System;

public class PlayerControlsV2 : MonoBehaviour {
	
	public float moveSpeed = 5;
	public float rotateSpeed = 3;
	public float jumpStrength = 20;
	public float msScaling = 2;
	private float baseMoveSpeed;
	private Rigidbody rb;
	private bool grounded = false;
	private float gravityCons = 5;
	private string[] allowedJump;
	private bool canMove;
	private string[] allowedObjs;
	
	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		rb.freezeRotation = true;
		allowedObjs = new string[4]{"Stage", "Ceiling", "Destructible", "NPC"};
		allowedJump = new string[3]{"Stage", "Destructible", "NPC"};
		baseMoveSpeed = moveSpeed;
		canMove = true;
	}
	
	public void toggleCanMove(){
		canMove = !canMove;
	}
	
	void OnCollisionEnter(Collision c){
		if (Array.IndexOf(allowedJump, c.collider.tag) >= 0) {
			grounded = true;
		}
	}
	
	private void updateSpeed(){
		float newMovespeed = baseMoveSpeed + msScaling * Mathf.Ceil (transform.localScale.x);
		if (moveSpeed < newMovespeed) {
			moveSpeed = newMovespeed;
		}
	}
	
	void Update () {
		if (canMove) {
			updateSpeed ();
			
			float horz = Input.GetAxis ("Horizontal");
			float vert = Input.GetAxis ("Vertical");
			float hop = Input.GetAxis ("Jump");

			rb.velocity = new Vector3(transform.forward.x * vert * moveSpeed, 
			                          this.rb.velocity.y, 
			                          transform.forward.z * vert * moveSpeed);

			transform.eulerAngles = new Vector3 (transform.localRotation.eulerAngles.x,
			                                     transform.localRotation.eulerAngles.y + horz * rotateSpeed,
			                                     transform.localRotation.eulerAngles.z);
			rb.mass = transform.localScale.x;


			float checkDis = this.transform.localScale.x;
			Vector3 velo = rb.velocity;
			velo.y = 0;
			Ray r = new Ray (this.transform.position, velo);
			RaycastHit hit; 
			if (Physics.Raycast(r, out hit, checkDis)) {
				Debug.DrawRay(transform.position, velo);
				print (hit.collider.tag);
				if (Array.IndexOf(allowedObjs, hit.collider.tag) >= 0){
					rb.velocity = new Vector3(0, rb.velocity.y, 0);
				}
			}


			if (hop > 0 && grounded) {
				float nJS = transform.localScale.x * jumpStrength;
				rb.AddForce (Vector3.up * nJS, ForceMode.Impulse);
				grounded = false;
			}
			
			if (!grounded) {
				applyExtraGravity ();
			}
		}
	}
	void applyExtraGravity(){
		gravityCons = transform.localScale.x * 2 * jumpStrength;
		rb.AddForce (Vector3.down * gravityCons);
	}


	/*Sources
	 * http://forum.unity3d.com/threads/object-wont-fall-when-i-apply-horizontal-velocity-and-is-colliding-with-wall.143698/
	 * 
     */
}
