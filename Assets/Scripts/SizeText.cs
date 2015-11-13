using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SizeText : MonoBehaviour {

	public GameObject displayManagerObject;
	private DisplayManager dm;
	private Text t;
	private float currVol, nextVol;

	void Start () {
		dm = displayManagerObject.GetComponent<DisplayManager> ();
		currVol = 0;
		nextVol = 0;
		t = this.GetComponent<Text> ();
	}

	void Update () {
		if (currVol != dm.getScaleProgression() || nextVol != dm.getScaleDifference()) {
			currVol = dm.getScaleProgression();
			nextVol = dm.getScaleDifference();
			string newStr, cvStr, nvStr;
			if (currVol > 1000){
				cvStr = (currVol / 1000).ToString("F2") + "k";
			}else{
				cvStr = currVol.ToString("F2");
			}
			if (nextVol > 1000){
				nvStr = (nextVol / 1000).ToString("F2") + "k";
			}else{
				nvStr = nextVol.ToString("F2");
			}
			newStr = cvStr + "/" + nvStr;
			t.text = newStr;
		}
	}
}
