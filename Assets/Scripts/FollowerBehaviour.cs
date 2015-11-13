using UnityEngine;
using System.Collections;

public class FollowerBehaviour : MonoBehaviour {

	public GameObject player;
	private PlayerControls pc;
	private Rigidbody rb;
	private float followDistanceMult;
	private float walkSpeed = 10;
	private float maxDis;
	public bool isFollowing = false;

	void Start () {
		pc = player.GetComponent <PlayerControls>();
		followDistanceMult = 1.3f;
		rb = GetComponent<Rigidbody> ();
	}

	void move(){
		walkSpeed = pc.moveSpeed * 0.25f;

		Vector3 target = player.transform.position;
		float followDistance = Mathf.Max(((player.transform.localScale.x /2) + (transform.localScale.x/2)) * followDistanceMult, 5f);
		float distanceBetween = Vector3.Distance (transform.position, target)
			- (player.transform.localScale.x / 2) - (transform.localScale.x * 1.5f);
		if (distanceBetween > followDistance) {
			Vector3 desiredForward = (target - transform.position).normalized;
			desiredForward.y = 0;
			transform.forward = desiredForward;

			rb.velocity = new Vector3 (transform.forward.x * walkSpeed, 
			                           this.rb.velocity.y, 
			                           transform.forward.z * walkSpeed);
			transform.Rotate (new Vector3 (0, -90, 0));
		} else {
			Vector3 temp = rb.velocity;
			temp.x = 0;
			temp.z = 0;
			rb.velocity = temp;
		}

		if (rb.velocity.y > 10) {
			Vector3 newVel = rb.velocity;
			newVel.y = 10;
			rb.velocity = newVel;
		}
		maxDis = player.transform.localScale.x * 20;
		if (distanceBetween > maxDis) {
			Vector3 newPos = player.transform.position;
			newPos.y += player.transform.localScale.x * 2;
			transform.position = newPos;
		}
	}

	void Update () {
		if (isFollowing) {
			move();
		}
	}
}
