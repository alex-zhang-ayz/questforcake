using UnityEngine;
using System.Collections;

public class WaterAnimation : MonoBehaviour {

	public GameObject finalObjParent;
	public bool finished = false;
	private CameraBehaviour cb;
	private int anindex = 0;
	private Transform workingChild;
	private Vector3 targetPos, targetRotation, targetScale;
	private float posSpeed = 2.5f;
	private float roSpeed = 2.5f;
	private float scSpeed = 2.5f;
	

	void Start () {
		setTargets ();
	}
	
	void setTargets(){
		Transform target = finalObjParent.transform.GetChild (anindex);
		targetPos = target.position;
		targetRotation = target.eulerAngles;
		targetScale = target.localScale;
		transform.GetChild (anindex).gameObject.SetActive (true);
		workingChild = transform.GetChild (anindex);
	}

	void updateChild(){
		workingChild.position = Vector3.Lerp (workingChild.position, targetPos, posSpeed * Time.deltaTime);
		workingChild.eulerAngles = Vector3.Lerp (workingChild.eulerAngles, targetRotation, roSpeed * Time.deltaTime);
		workingChild.localScale = Vector3.Lerp (workingChild.localScale, targetScale, scSpeed * Time.deltaTime);
		if (Mathf.Round (workingChild.position.magnitude * 10)/ 10 == Mathf.Round (targetPos.magnitude * 10) / 10) {
			anindex++;
			if (anindex < transform.childCount){
				setTargets();
			}
		}
	}

	void Update () {
		if (anindex < transform.childCount) {
			updateChild ();
		} else {
			finished = true;
		}
	}
}
