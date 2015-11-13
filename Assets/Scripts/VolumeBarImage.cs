using UnityEngine;
using System.Collections;

public class VolumeBarImage : MonoBehaviour {

	public GameObject displayManager;
	private float yPos;
	private RectTransform rt;
	private DisplayManager dm;

	void Start () {
		dm = displayManager.GetComponent<DisplayManager> ();
		rt = this.GetComponent<RectTransform> ();
		yPos = (-1) * rt.rect.height;
	}

	private void updateImage(){
		yPos = rt.rect.height * dm.getPercentVolume () - rt.rect.height;
		if (this.transform.localPosition.y != yPos) {
			this.transform.localPosition = new Vector3 (this.transform.localPosition.x,
			                                            yPos,
			                                            this.transform.localPosition.z);
			if (this.transform.localPosition.y > 0) {
				this.transform.localPosition = new Vector3 (this.transform.localPosition.x,
				                                            (-1) * rt.rect.height,
				                                            this.transform.localPosition.z);
			}
		}
	}

	void Update () {
		updateImage ();
	}
}
