using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public GameObject[] panels;

	void Start () {
		for (int i=0; i<panels.Length; i++) {
			panels[i].SetActive(false);
		}
	}
	public void openPanel(int index){
		if (index < panels.Length) {
			for (int i=0; i<panels.Length; i++) {
				if (i != index){
					panels[i].SetActive(false);
				}else{
					panels[i].SetActive(true);
				}
			}
		} else {
			Debug.Log("index out of range, not enough objects in the array fool.");
		}
	}

	public void showPanel(int index){
		if (index < panels.Length) {
			for (int i=0; i<panels.Length; i++) {
				if (i != index){
					panels[i].SetActive(false);
				}else{
					panels[i].SetActive(!panels[i].activeSelf);
				}
			}
		} else {
			Debug.Log("index out of range, not enough objects in the array fool.");
		}
	}


	void Update () {
	
	}
}
