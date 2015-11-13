using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiameterText : MonoBehaviour {

	public GameObject displayManager;
	private DisplayManager dm;
	private Text t;
	
	void Start () {
		dm = displayManager.GetComponent<DisplayManager> ();
		t = this.GetComponent<Text> ();
	}

	void Update () {
		string newText;
		if (dm.getDiameter () > 1000) {
			t.fontSize = 12;
			newText = "D" + (dm.getDiameter() / 1000.0).ToString("F2") + "k";
		} else {
			newText  = "D" + dm.getDiameter ();
		}
		if (t != null && t.text != newText) {
			t.text = newText;
		}
	}
}
