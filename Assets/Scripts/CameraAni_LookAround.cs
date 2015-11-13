using UnityEngine;
using System.Collections;

public class CameraAni_LookAround : MonoBehaviour, CameraAnimation {
	
	public float moveSpeed = 1;
	public float rotateSpeed = 1;
	public GameObject exitObjective;
	public GameObject objectivesObj;
	public GameObject volumeUIgbt;
	public GameObject viewObject;
	public GameObject dcObject;
	private DestroyControls dc;
	private ObjectiveHandler oh;
	private bool startAni, finished;
	private Vector3[] targetLocations;
	private Vector3[] targetRotations;
	private int whichTarget;
	private int targetCount;

	private bool doOnce = false;
	private bool aDoOnce = false;

	private float startTime;
	private float delay = 10;

	public bool canPlayAnimation = true;

	// Use this for initialization
	void Start () {
		dc = dcObject.GetComponent<DestroyControls> ();
		oh = objectivesObj.GetComponent<ObjectiveHandler> ();
		whichTarget = 0;
		targetCount = viewObject.transform.childCount;
		startAni = false;
		finished = false;
		targetLocations = new Vector3[viewObject.transform.childCount + 1];
		targetRotations = new Vector3[viewObject.transform.childCount + 1];
		int index = 0;
		foreach (Transform child in viewObject.transform) {
			targetLocations[index] = child.position;
			targetRotations[index] = child.localRotation.eulerAngles;
			index++;
		}
		if (!canPlayAnimation) {
			finished = true;
		}
	}

	public void playAnimation(){
		startAni = true;
	}

	public bool getFinished(){
		return finished;
	}

	public void sendFinalPos(Vector3 location, Vector3 rotation){
		targetLocations [viewObject.transform.childCount] = location;
		targetRotations [viewObject.transform.childCount] = rotation;
		targetCount++;
	}

	private void move(){
		if (whichTarget < targetCount) {
			Vector3 targetPos = targetLocations [whichTarget];
			Vector3 targetRotation = targetRotations[whichTarget];

			Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, targetPos, Time.deltaTime * moveSpeed);
			Vector3 roundedCameraPos = new Vector3(Mathf.Round(Camera.main.transform.position.x),
			                                       Mathf.Round(Camera.main.transform.position.y),
			                                       Mathf.Round(Camera.main.transform.position.z));
			Vector3 roundedTargetPos = new Vector3(Mathf.Round(targetPos.x),
			                                       Mathf.Round(targetPos.y),
			                                       Mathf.Round(targetPos.z));
			Vector3 ceilCameraPos = new Vector3(Mathf.Ceil(Camera.main.transform.position.x),
			                                       Mathf.Ceil(Camera.main.transform.position.y),
			                                       Mathf.Ceil(Camera.main.transform.position.z));
			Vector3 floorCameraPos = new Vector3(Mathf.Floor(Camera.main.transform.position.x),
			                                       Mathf.Floor(Camera.main.transform.position.y),
			                                       Mathf.Floor(Camera.main.transform.position.z));
			//print ("CameraPos: " + roundedCameraPos);
			//print ("TargetPos: " + roundedTargetPos);

			Camera.main.transform.localEulerAngles = Vector3.Lerp (Camera.main.transform.localEulerAngles, targetRotation, Time.deltaTime * rotateSpeed);
			Vector3 roundedCameraRot = new Vector3(Mathf.Round(Camera.main.transform.localEulerAngles.x),
			                                       Mathf.Round(Camera.main.transform.localEulerAngles.y),
			                                       Mathf.Round(Camera.main.transform.localEulerAngles.z));
			Vector3 roundedTargetRot = new Vector3(Mathf.Round(targetRotation.x),
			                                       Mathf.Round(targetRotation.y),
			                                       Mathf.Round(targetRotation.z));
			
			//print ("CameraPos: " + roundedCameraPos);
			//print ("TargetPos: " + roundedTargetPos);

			if ((Vector3.Equals(roundedCameraPos, roundedTargetPos) || Vector3.Equals(ceilCameraPos, roundedTargetPos) || Vector3.Equals(floorCameraPos, roundedTargetPos))
			    && Vector3.Equals(roundedCameraRot, roundedTargetRot)){
				startTime = Time.time;
				whichTarget++;
			}
			print (Time.time - startTime);
			if (Time.time - startTime > delay){
				print ("passed delay");
				whichTarget++;
			}
		} else {
			finished = true;
		}
	}

	void Update () {
		if (startAni && !finished) {
			if (!aDoOnce){
				startTime = Time.time;
				aDoOnce = true;
			}
			dc.checkOverUI = false;
			dc.destroyMode = false;
			move ();
		} else if (!doOnce) {
			oh.addObjective(exitObjective);
			doOnce = true;
		}
	}
}
