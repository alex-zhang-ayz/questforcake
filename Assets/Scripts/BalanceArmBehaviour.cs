using UnityEngine;
using System.Collections;

public class BalanceArmBehaviour : MonoBehaviour {

	public GameObject scale1;
	public GameObject scale2;
	public GameObject objGroup1;
	public GameObject objGroup2;
	private ScaleGroupBehaviour sgb1, sgb2;

	private Vector3 scalePos1, scalePos2;

	[HideInInspector]
	public bool isBalanced = false;

	void Start () {
		sgb1 = objGroup1.GetComponent<ScaleGroupBehaviour> ();
		sgb2 = objGroup2.GetComponent<ScaleGroupBehaviour> ();
	}


	Vector3 getNewArmAngle(){
		float g1Vol = sgb1.getGroupVolume (); 
		float g2Vol = sgb2.getGroupVolume ();
		if (g1Vol >= 0 && g2Vol >= 0) {
			float percent = 0.5f;
			float angle = 0;
			if (g1Vol > g2Vol) {
				percent = 1 - (g2Vol / g1Vol);
				angle = (percent * 40);
			} else if (g1Vol < g2Vol) {
				percent = g1Vol / g2Vol;
				angle = (percent * 40) - 40;
			}
			if (g1Vol != 0 && g2Vol != 0){
				float r1 = Mathf.Round(g1Vol / 10) * 10;
				float r2 = Mathf.Round(g2Vol/ 10) * 10;
				isBalanced = (g1Vol.ToString("F5") == g2Vol.ToString("F5"))
					|| r1 == r2;
				//print (string.Format("r1: {0}\nr2: {1}\n", r1, r2));
				//print (isBalanced);
			}
			//print (string.Format ("g1Vol: {0}\ng2Vol: {1}\n", g1Vol.ToString("F1"), g2Vol.ToString("F1")));
			Vector3 newEular = new Vector3 (0, 0, angle);
			return newEular;
		} else {
			return Vector3.one;
		}

	}

	void updateArmAngle (){
		Vector3 targetEular = getNewArmAngle ();
		if (!Vector3.Equals (targetEular, Vector3.one)){
			float lerpSpeed = 1;
			Vector3 toNextLerp = Vector3.Lerp (transform.localEulerAngles, targetEular, lerpSpeed * Time.deltaTime);
			if (toNextLerp.z > 40 || toNextLerp.z < -40){
				transform.localEulerAngles = targetEular;
			}else{
				transform.localEulerAngles = toNextLerp;
			}
			Vector3 left_point = transform.position - (transform.right.normalized * transform.lossyScale.x / 2);
			Vector3 right_point = transform.position + (transform.right.normalized * transform.lossyScale.x / 2);
			scale1.transform.position = left_point;
			scale2.transform.position = right_point;
		}
	}

	void fixAngle(){
		if (transform.eulerAngles.z > 180) {
			Vector3 nEu = transform.eulerAngles;
			nEu.z -= 360;
			transform.eulerAngles = nEu;
		}
	}

	void updateScalePos(){
		if (scalePos1 != null && Vector3.Equals(scalePos1, scale1.transform.position)) {
			scale1.transform.position = scalePos1;
		}
		if (scalePos2 != null && Vector3.Equals(scalePos2, scale2.transform.position)) {
			scale2.transform.position = scalePos2;
		}
	}

	void Update () {
		//fixAngle ();
		updateArmAngle ();
		updateScalePos ();
	}
}
