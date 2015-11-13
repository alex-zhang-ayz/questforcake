using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InputController : MonoBehaviour {

	public static InputController instance;
	private Dictionary<string, string[]> controlsDict;

	void Awake(){

		if (instance == null) {
			instance = this;
		} else if (instance != this){
			DestroyImmediate(this);
		}
		controlsDict = new Dictionary<string, string[]> ();
		defaultValues ();
	}

	void Start () {

	}

	public void changeValue(string controlSet, int index, string key){
		controlsDict [controlSet] [index] = key;
	}
	

	private void defaultValues(){
		string[] ctrls = new string[4];
		//Horizontal
		ctrls [0] = "A";
		ctrls [1] = "LeftArrow";
		ctrls [2] = "D";
		ctrls [3] = "RightArrow";
		controlsDict.Add ("Horizontal", ctrls);
		//Vertical
		ctrls = new string[4];
		ctrls [0] = "S";
		ctrls [1] = "DownArrow";
		ctrls [2] = "W";
		ctrls [3] = "UpArrow";
		controlsDict.Add ("Vertical", ctrls);
		//Click
		ctrls = new string[1];
		ctrls [0] = "Left Click";
		controlsDict.Add ("Click", ctrls);
		//Freelook
		ctrls = new string[1];
		ctrls [0] = "Escape";
		controlsDict.Add ("Freelook", ctrls);
		//Restart
		ctrls = new string[1];
		ctrls [0] = "F12";
		controlsDict.Add ("Restart", ctrls);
	}

	public string[] getValue(string s){
		return controlsDict [s];
	}

	void Update () {
		DontDestroyOnLoad (this);
	}
}
