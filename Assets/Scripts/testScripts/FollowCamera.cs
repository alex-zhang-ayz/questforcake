using UnityEngine;
using System.Collections;
using System;

public class FollowCamera : MonoBehaviour {

	public GameObject player;

	public float turnSpeed = 1f;
	public float distanceMult = 5f;
	private float height, distance;
	private float length = 10f;
	private Vector3 offsetX;
	private string[] allowedObjs;
	
	public float XSensitivity = 0.5f;
	public float YSensitivity = 0.5f;
	private Vector3 forward;
	private Vector3 targetRotation;

	void Start () {
		forward = this.transform.forward;
		updateOffset ();
		allowedObjs = new string[2]{"Player", "Ceiling"};
		length = Mathf.Sqrt (height * height + distance * distance) - player.transform.localScale.x / 2.0f;

		targetRotation = Vector3.zero;
	}

	private void updateOffset(){
		height = player.transform.localScale.x * distanceMult * 0.5f;
		distance = player.transform.localScale.x * distanceMult;
		length = Mathf.Sqrt (height * height + distance * distance) - player.transform.localScale.x / 2.0f;
		Vector3 back;
		back = forward.normalized * (-1) * distance;
		offsetX = new Vector3(back.x, height,  back.z);
	}
	
	private void resetRotation(){
		transform.localEulerAngles = new Vector3 (0, transform.localRotation.eulerAngles.y,
		                                          transform.localRotation.eulerAngles.z);
	}
	

	private void changeForward(){
		float playerY = player.transform.eulerAngles.y;
		targetRotation = new Vector3 (transform.eulerAngles.x,
		                           	  playerY,
		                              transform.eulerAngles.z);
		transform.localEulerAngles = targetRotation;
		//transform.localEulerAngles = Vector3.Lerp (transform.localEulerAngles, 
	                                          // targetRotation, 
	                                          // turnSpeed);
	}

	private void cameraPlayerControls(){
		if (height != player.transform.localScale.x * distanceMult) {
			resetRotation ();
			updateOffset ();
		}

		transform.position = player.transform.position + offsetX; 
		Vector3 newHeight = new Vector3 (player.transform.position.x,
		                                 player.transform.position.y + player.transform.localScale.y,
		                                 player.transform.position.z);
		transform.LookAt (newHeight);
		changeForward ();
		forward = this.transform.forward;


		Ray r = new Ray (transform.position, transform.forward);
		Ray upRay = new Ray (player.transform.position, player.transform.up);
		RaycastHit bHit, uHit;
		Debug.DrawRay (player.transform.position, player.transform.up);
		bool behindRayCast = Physics.Raycast (r, out bHit, length);
		bool upRayCast = Physics.Raycast (upRay, out uHit, length);
		if (behindRayCast && !upRayCast) {
			if (Array.IndexOf (allowedObjs, bHit.collider.gameObject.tag) < 0) {
				print ("first!");
				
				Vector3 closestPoint = new Vector3 (bHit.point.x, player.transform.position.y, bHit.point.z);
				
				Vector3 midpoint = new Vector3 ((player.transform.position.x + closestPoint.x) / 2,
				                                (player.transform.position.y + closestPoint.y) / 2,
				                                (player.transform.position.z + closestPoint.z) / 2);

				float newH;
				if (bHit.point.y >= bHit.collider.transform.position.y) {
					float yDiff = player.transform.position.y - bHit.collider.transform.position.y;
					float remaining = bHit.collider.transform.localScale.y / 2 - yDiff;
					newH = remaining * 1.75f;
				} else {
					float yDiff = bHit.collider.transform.position.y - player.transform.position.y;
					float total = bHit.collider.transform.localScale.y / 2 + yDiff;
					newH = total * 1.75f;
				}
				
				Vector3 ffsetX = midpoint + new Vector3 (0, newH, 0);
				
				transform.position = ffsetX; 
				transform.LookAt (player.transform.position);
			}
		} else if (behindRayCast && upRayCast) {
			if (Array.IndexOf (allowedObjs, bHit.collider.gameObject.tag) < 0
				&& Array.IndexOf (allowedObjs, uHit.collider.gameObject.tag) < 0) {
				print ("second!");
				float maxHeight = uHit.point.y;
				float avgH = (maxHeight + player.transform.position.y) / 2;
				Vector3 closestPoint = new Vector3 (bHit.point.x, player.transform.position.y, bHit.point.z);
				
				Vector3 midpoint = new Vector3 ((player.transform.position.x + closestPoint.x) / 2,
				                                (player.transform.position.y + closestPoint.y) / 2,
				                                (player.transform.position.z + closestPoint.z) / 2);

				Vector3 ffsetX = midpoint + new Vector3 (0, avgH, 0);
				
				transform.position = ffsetX; 
				transform.LookAt (player.transform.position);
			}
		}
	}

	void Update () {
		cameraPlayerControls();
	}
}
