using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TotalVolumeText : MonoBehaviour {

	public GameObject playerVolumeObject;
	private PlayerVolume pv;
	private Text t;

	void Start () {
		pv = playerVolumeObject.GetComponent<PlayerVolume> ();
		t = this.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		string vStr;
		if (pv.getCurrScaleVolume () > 1000) {
			vStr = (pv.getCurrScaleVolume () / 1000).ToString ("F2") + "k";
		} else {
			vStr = pv.getCurrScaleVolume ().ToString ("F2");
		}
		if (t.text != vStr) {

			t.text = vStr;
		}
		/*
		if (t.text != pv.getCurrentVolume ().ToString ("F2")) {
			t.text = pv.getCurrentVolume().ToString("F2");
		}
		*/
	}
}
