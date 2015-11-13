using UnityEngine;
using System.Collections;

public class MusicAdjust : MonoBehaviour {

	public GameObject player;
	public GameObject yPlane;
	public GameObject audioObject;
	private AudioSource aus;
	private float baseY;

	private float baseVol;
	private float basePitch;

	void Start () {
		baseY = yPlane.transform.position.y;
		aus = audioObject.GetComponent<AudioSource> ();
		baseVol = aus.volume;
		basePitch = aus.pitch;
	}
	

	void Update () {
		if (baseY - player.transform.position.y >= 0) {
			print ("Base: " +baseY);
			print ("Player: " +player.transform.position.y);
			float posBase = Mathf.Abs (baseY);
			float diff = baseY - player.transform.position.y;
			float nPercent = diff / posBase;
			if (nPercent <= 1) {
				float targetPitch, targetVol;
				targetPitch = (1 - nPercent) * basePitch;
				targetVol = (1 - nPercent) * baseVol;
				if (targetPitch < 0.5f){
					targetPitch = 0.5f;
				}
				if (targetVol < 0.5f){
					targetVol = 0.5f;
				}
				aus.pitch = targetPitch;
				aus.volume = targetVol;
			}
		} else {
			aus.pitch = basePitch;
			aus.volume = baseVol;
		}
	}
}
