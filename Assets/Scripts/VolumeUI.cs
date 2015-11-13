using UnityEngine;
using System.Collections;

public class VolumeUI : MonoBehaviour {
	
	private bool showAll;
	
	void Start () {
		showAll = false;
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}

	}

	public bool getShowAll(){
		return showAll;
	}

	public void toggleAll(){
		showAll = !showAll;
		foreach (Transform child in transform) {
			child.gameObject.SetActive(showAll);
		}
	}
	
	void Update () {
		
	}
}
