using UnityEngine;
using System.Collections;

public class SnapBackDB : MonoBehaviour {

	public GameObject startPlatform;
	private Vector3 startPos;
	private float startTime = -1;
	private float delay = 20;
	private Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		startPos = transform.position;
	}

	void OnCollisionEnter(Collision c){
		if (c.gameObject == startPlatform) {
			print ("entered platform");
			startTime = -1;
		}
	}

	void OnCollisionExit(Collision c){
		if (c.gameObject == startPlatform) {
			print ("exited platform");
			startTime = Time.time;
		}
	}

	void Update () {
		if (startTime > 0 && Time.time - startTime > delay){
			transform.position = startPos;
			rb.velocity = Vector3.zero;
			startTime = -1;
		}
	}
}
