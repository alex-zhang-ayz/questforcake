using UnityEngine;
using System.Collections;

public class ControlMenu : MonoBehaviour {

	private bool showMenu;

	public GameObject controlPanel;

	void Start () {
		showMenu = false;
		controlPanel.SetActive (false);
	}

	public void toggleMenu(){
		showMenu = !showMenu;
		controlPanel.SetActive (showMenu);
	}

	void Update () {

	}
}
