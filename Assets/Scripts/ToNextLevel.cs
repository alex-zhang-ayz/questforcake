using UnityEngine;
using System.Collections;

public class ToNextLevel : MonoBehaviour {

	public string levelName = "";
	public string pgd = "PreservedGameData";
	private CursorMode cursorMode = CursorMode.Auto;

	void Start () {
		resetCursor ();
	}

	public void gotoLevel(){
		if (levelName != "") {
			Application.LoadLevel(levelName);
		}
	}

	private void resetCursor (){
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}

	private void resetPGD(){
		GameObject g = GameObject.Find (pgd);
		if (g != null) {
			PreservedGameData p = g.GetComponent<PreservedGameData> ();
			if (p != null){
				p.reset();
			}
		}
	}

	void Update () {
		resetPGD ();
	}
}
