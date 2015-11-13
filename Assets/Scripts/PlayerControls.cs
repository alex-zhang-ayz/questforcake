using UnityEngine;
using System.Collections;
using System;

/*Sources
http://forum.unity3d.com/threads/object-wont-fall-when-i-apply-horizontal-velocity-and-is-colliding-with-wall.143698/

 */

public class PlayerControls : MonoBehaviour {


	public float moveSpeed = 5;
	public float rotateSpeed = 3;
	public float jumpStrength = 20;
	public float msScaling = 10;
	public bool restrictLevelOne = false;
	public string inputControllerName = "InputController";
	private InputController ic;
	private float baseMoveSpeed;
	private Rigidbody rb;
	private bool grounded = false;
	private float gravityCons = 5;
	private string[] allowedJump;
	private bool canMove;
	private string[] cameraClip;

	private int maxJumps = 1;
	private float prevJump;
	private int jumpCount;
	//private float prevFLM;
	private bool prevFLM;


	private bool freeLookMode;

	void Start () {
		GameObject icg = GameObject.Find (inputControllerName);
		ic = icg.GetComponent<InputController> ();

		rb = this.GetComponent<Rigidbody> ();
		rb.freezeRotation = true;
		cameraClip = new string[3]{"Stage", "Ceiling", "Destructible"};
		allowedJump = new string[3]{"Stage", "Destructible", "NPC"};
		baseMoveSpeed = moveSpeed;
		canMove = true;
		jumpCount = maxJumps;
		prevJump = 0;
		//prevFLM = 0;
		prevFLM = false;
		freeLookMode = false;
	}

	public void toggleCanMove(){
		Vector3 newVel = rb.velocity;
		newVel.x = 0;
		newVel.z = 0;
		rb.velocity = newVel;
		canMove = !canMove;
	}

	void OnCollisionEnter(Collision c){
		if (Array.IndexOf(allowedJump, c.collider.tag) >= 0) {
			jumpCount = maxJumps;
			grounded = true;
		}
	}

	private void updateSpeed(){
		float newMovespeed = baseMoveSpeed + msScaling * Mathf.Ceil (transform.localScale.x);
		if (moveSpeed < newMovespeed) {
			moveSpeed = newMovespeed;
		}
	}

	private void toggleFreeLookMode(){
		freeLookMode = !freeLookMode;
	}

	public bool getFreeLookMode(){
		return freeLookMode;
	}

	private void checkRestart(){
		string[] rst = ic.getValue ("Restart");
		foreach (string s in rst) {
			if (Input.GetKey((KeyCode) System.Enum.Parse(typeof(KeyCode), s) )){
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	private void checkFreelook(){
		bool flm = false;
		string[] fl = ic.getValue ("Freelook");
		foreach (string s in fl) {
			if (Input.GetKey((KeyCode) System.Enum.Parse(typeof(KeyCode), s) )){
				flm = true;
			}
		}
		if (!prevFLM && flm) {
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
			toggleFreeLookMode();
		}
		prevFLM = flm;
	}

	void Update () {
		checkRestart ();
		checkFreelook ();
		/*
		if (Input.GetKey (KeyCode.F12)) {
			Application.LoadLevel(Application.loadedLevel);
		}


		float flm = Input.GetAxis ("Cancel");
		if (prevFLM == 0 && flm > 0) {
			rb.velocity = new Vector3(0, rb.velocity.y, 0);
			toggleFreeLookMode();
		}
		prevFLM = flm;
		*/


		if (canMove) {
			updateSpeed ();

			float horz = Input.GetAxis ("Horizontal");
			float vert = Input.GetAxis ("Vertical");
			float hop = Input.GetAxis ("Jump");
			hop = 0;		//!!!!!!!

			rb.velocity = new Vector3 (transform.forward.x * vert * moveSpeed, 
			                          this.rb.velocity.y, 
			                          transform.forward.z * vert * moveSpeed);
			
			transform.eulerAngles = new Vector3 (transform.localRotation.eulerAngles.x,
			                                     transform.localRotation.eulerAngles.y + horz * rotateSpeed,
			                                     transform.localRotation.eulerAngles.z);
			
			float checkDis = (this.transform.localScale.x / 2);
			Vector3 velo = rb.velocity.normalized;
			velo.y = 0;
			RaycastHit hit;

			Ray r = new Ray (this.transform.position, velo);
			if (Physics.Raycast (r, out hit, checkDis)) {
				Debug.DrawLine(transform.position, hit.point);
				DestructibleBehaviour db = hit.collider.gameObject.GetComponent<DestructibleBehaviour> ();

				Debug.DrawRay (transform.position, velo);
				if (db != null && db.isWall && Array.IndexOf (cameraClip, hit.collider.tag) >= 0) {
					rb.velocity = new Vector3 (0, rb.velocity.y, 0);
				}
			}

			
			if (prevJump == 0 && hop > 0 && jumpCount > 0) {
				float nJS = transform.localScale.x * jumpStrength;
				if (transform.localScale.x == 1 && restrictLevelOne){
					jumpCount = 1;
					nJS = 7;
				}
				rb.AddForce (Vector3.up * nJS, ForceMode.Impulse);
				jumpCount --;
				grounded = false;
			}
			prevJump = hop;
		
			applyExtraGravity ();

		}
	}
	void applyExtraGravity(){
		gravityCons = transform.localScale.x * 1.5f * jumpStrength * transform.localScale.x;
		rb.AddForce (Vector3.down * gravityCons);
	}
}
