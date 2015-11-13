using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoverObjectBehaviour : MonoBehaviour {

	private Text hoverText;
	private bool wakeStatus;
	private int currentWorkingID;

	void Start () {
		hoverText = transform.GetChild (0).GetComponent<Text> ();
		wakeStatus = false;
		gameObject.SetActive (false);
	}

	public void setOn(string s, int cWID){
		if (!wakeStatus && s != null) {
			currentWorkingID = cWID;
			gameObject.SetActive (true);
			hoverText = transform.GetChild (0).GetComponent<Text> ();
			hoverText.text = s;
			wakeStatus = true;
		}
	}

	public void setOff(int ID){
		if (wakeStatus && ID == currentWorkingID) {
			hoverText.text = "";
			wakeStatus = false;
			gameObject.SetActive(false);
		}
	}

	void Update () {
		transform.position = Input.mousePosition;
		if (Input.GetMouseButtonDown (0)) {
			setOff (currentWorkingID);
		}
	}
}
