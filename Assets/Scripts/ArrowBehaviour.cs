using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {

	public GameObject player;
	private float height;
	private GameObject target;
	private Vector3 baseRotate;

	void Start () {
		baseRotate = transform.eulerAngles;
		height = player.transform.localScale.y / 2;
	}

	public void setTarget(GameObject g){
		target = g;
	}

	private void positionArrow(){
		/*
		Vector3 targetNDir = (target.transform.position - player.transform.position).normalized;
		Debug.DrawRay (player.transform.position, targetNDir);
		float totalDis = (player.transform.localScale.x) / 2f + distance;
		Vector3 scale = Vector3.one * totalDis;
		Vector3 newPos = Vector3.Scale (targetNDir, scale);
		transform.position = newPos + player.transform.position;
		*/
		height = player.transform.localScale.y / 2;
		Vector3 addHeight = new Vector3 (0, player.transform.localScale.y / 2 + height, 0);
		transform.position = player.transform.position + addHeight;
	}

	private void rotateArrow(){
		Vector3 diff = target.transform.position - player.transform.position;
		transform.rotation = Quaternion.LookRotation(diff.normalized);
		transform.Rotate (baseRotate);
	}

	private void scaleArrow(){
		Vector3 scaling = Vector3.one * 0.15f;
		transform.localScale = Vector3.Scale(player.transform.localScale, scaling);
	}

	void Update () {
		scaleArrow ();
		if (target != null) {
			positionArrow();
			rotateArrow();
		}
	}
}
