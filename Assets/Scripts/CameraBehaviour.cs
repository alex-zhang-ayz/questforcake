using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CameraBehaviour : MonoBehaviour {

	public GameObject player;
	public GameObject destroyControlsObj;
	public GameObject startAnimationObject;
	private DestroyControls dc;
	private PlayerControls pc;
	public float turnSpeed = 1f;
	public float distanceMult = 5f;
	private float height, distance;
	private float length = 10f;
	private Vector3 offsetX;
	private string[] allowedObjs;
	private bool atAniFinish;
	private bool doOnce;

	public float XSensitivity = 0.5f;
	public float YSensitivity = 0.5f;

	private Vector3 forward;
	private Vector3 targetRotation;
	private bool playedStart;
	private CameraAnimation cameraAni;
	private Vector3 savedScale = Vector3.one;

	private bool isTempLook;

	void Start () {
		atAniFinish = false;
		pc = player.GetComponent<PlayerControls> ();
		dc = destroyControlsObj.GetComponent<DestroyControls> ();
		cameraAni = startAnimationObject.GetComponent<CameraAnimation> ();
		playedStart = false;
		doOnce = false;
		isTempLook = false;
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
		/*
		transform.localEulerAngles = Vector3.Lerp (transform.localEulerAngles, 
		                                           targetRotation, 
		                                           turnSpeed);
		*/
		
	}

	private void cameraPlayerControls(){
		//if (height != player.transform.localScale.x * distanceMult) {
		if (height != player.transform.localScale.x * distanceMult || 
		    savedScale != player.transform.localScale){
			resetRotation ();
			updateOffset ();
			savedScale = player.transform.localScale;
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
				
				Vector3 closestPoint = new Vector3 (bHit.point.x, player.transform.position.y, bHit.point.z);
				
				Vector3 midpoint = new Vector3 ((player.transform.position.x + closestPoint.x) / 2,
				                                (player.transform.position.y + closestPoint.y) / 2,
				                                (player.transform.position.z + closestPoint.z) / 2);

				float d = Vector3.Distance(midpoint, player.transform.position);
				
				float newH;

				newH = Mathf.Pow(length * length - d * d, 0.5f);
				Vector3 ffsetX = midpoint + new Vector3 (0, newH, 0);
				
				transform.position = ffsetX; 
				transform.LookAt (player.transform.position);
			}
		} else if (behindRayCast && upRayCast) {
			if (Array.IndexOf (allowedObjs, bHit.collider.gameObject.tag) < 0
			    && Array.IndexOf (allowedObjs, uHit.collider.gameObject.tag) < 0) {

				float maxHeight = uHit.point.y;
				float avgH = (maxHeight + player.transform.position.y) / 2;
				Vector3 closestPoint = new Vector3 (bHit.point.x, player.transform.position.y, bHit.point.z);
				
				Vector3 midpoint = new Vector3 ((player.transform.position.x + closestPoint.x) / 2,
				                                player.transform.position.y,
				                                (player.transform.position.z + closestPoint.z) / 2);

				midpoint.y = avgH;

				transform.position = midpoint; 
				transform.LookAt (player.transform.position);
			}
		}
	}

	private void freeLookControls(){
		Vector3 scalingVector = Vector3.one * player.transform.localScale.x / 2f;
		Vector3 forwardNormal = player.transform.forward.normalized;
		Vector3 offset = Vector3.Scale (forwardNormal, scalingVector);

		transform.position = player.transform.position + offset;
		if (doOnce) {
			transform.eulerAngles = new Vector3 (0,
			                                  player.transform.localRotation.eulerAngles.y,
		                                         0);
			doOnce = false;
		}
		//float xRot = Input.GetAxis("Mouse X") * XSensitivity;
		float yRot = Input.GetAxis("Mouse Y") * YSensitivity;
		transform.eulerAngles = new Vector3 (transform.eulerAngles.x,
		                                     player.transform.localRotation.eulerAngles.y,
		                                     transform.eulerAngles.z);

		//transform.localRotation *= Quaternion.Euler (0f, yRot, 0f);
		transform.localRotation *= Quaternion.Euler (-yRot, 0f, 0f);

	}

	public void startTempLook(Vector3 pos, Vector3 eularAngles){
		transform.position = pos;
		transform.eulerAngles = eularAngles;
		pc.toggleCanMove ();
		isTempLook = true;
	}

	public void finishTempLook(){
		pc.toggleCanMove ();
		isTempLook = false;
	}

	void Update () {
		if (!playedStart) {
			cameraPlayerControls();
			cameraAni.sendFinalPos(transform.position, transform.localRotation.eulerAngles);
			cameraAni.playAnimation();
			pc.toggleCanMove();
			playedStart = true;
		}
		if (cameraAni.getFinished ()) {
			if (!atAniFinish){
				pc.toggleCanMove();
				atAniFinish = true;
			}
			if (!isTempLook){
				if (pc.getFreeLookMode()){
					freeLookControls();
				}else{
					doOnce = true;
					cameraPlayerControls();
				}
			}
		}

	}
}
