using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlText : MonoBehaviour {

	public string ctrl;
	private string[] keys;
	private ControlContainer cc;
	private Text ctrlText;

	void Start () {
		Transform t = ExtraFunctions.searchInAncestor (transform, typeof(ControlContainer));
		cc = t.GetComponent<ControlContainer> ();
		ctrlText = transform.GetChild (0).GetComponent<Text> ();
	}

	public void updateText (){
		string text = "";
		foreach (string s in keys) {
			text += "["+ s + "] ";
		}
		ctrlText.text = text;

	}

	void Update () {
		if (keys == null) {
			keys = cc.getKeys (ctrl);
		}
		updateText ();
	}
}
