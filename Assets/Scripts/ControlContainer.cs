using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class ControlContainer : MonoBehaviour {

	public string inputControllerName = "InputController";
	private InputController ic;
	private Dictionary<string, int> axisInfo;
	private Dictionary<string, string[]> ctrlInfo;

	void Start () {
		GameObject icg = GameObject.Find (inputControllerName);
		ic = icg.GetComponent<InputController> ();
		axisInfo = new Dictionary<string, int> ();
		ctrlInfo = new Dictionary<string, string[]> ();
		addInfo ();
		addCtrls();
	}

	void addInfo(){
		axisInfo.Add ("Horizontal", 0);
		axisInfo.Add ("Vertical", 1);
		axisInfo.Add ("Cancel", 17);
	}

	public string[] getKeys(string s){
		return ctrlInfo [s];
	}
	void addCtrls(){
		//Horizontal
		string[] neg = new string[2];
		string[] pos = new string[2];
		Array.Copy (ic.getValue("Horizontal"), 0, neg, 0, 2);
		Array.Copy (ic.getValue("Horizontal"), 2, pos, 0, 2);
		ctrlInfo.Add ("Right", pos);
		ctrlInfo.Add ("Left", neg);
		//Vertical
		neg = new string[2];
		pos = new string[2];
		Array.Copy (ic.getValue("Vertical"), 0, neg, 0, 2);
		Array.Copy (ic.getValue("Vertical"), 2, pos, 0, 2);
		ctrlInfo.Add ("Forward", pos);
		ctrlInfo.Add ("Backward", neg);
		//Interact/Shrink
		pos = ic.getValue("Click");
		ctrlInfo.Add ("Click", pos);
		//Free Look
		pos = ic.getValue ("Freelook");
		ctrlInfo.Add ("Freelook", pos);
		//Restart
		pos = ic.getValue("Restart");
		ctrlInfo.Add ("Restart", pos);
		
		
	}

	void Update () {

	}

}
