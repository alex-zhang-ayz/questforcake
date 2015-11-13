using UnityEngine;
using System.Collections;

public class SidebuttonBehaviour : MonoBehaviour {

	public bool tutMode = false;

	void Start () {
		if (tutMode) {
			tutStartButtons();
		}
	}

	private void tutStartButtons(){
		Transform settingsB = transform.GetChild (0);
		foreach (Transform child in transform) {
			if (child != settingsB){
				child.gameObject.SetActive(false);
			}
		}
		setButtonActive (3);
	}

	public void setButtonActive(int i){
		Transform child = transform.GetChild (i);
		child.gameObject.SetActive (true);
	}

	void checkMode(){

	}

	void Update () {
	
	}
}
