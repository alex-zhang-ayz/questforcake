using UnityEngine;
using System.Collections;

public class BG_Music : MonoBehaviour {

	private AudioSource aus;
	private float baseVolume;
	private float startTime;

	void Start () {
		aus = this.GetComponent<AudioSource> ();
		baseVolume = aus.volume;
		startTime = Time.time;
		aus.volume *= 0.25f;
	}
	

	void Update () {
		if (!aus.isPlaying) {
			if (Time.time - startTime > 2f){
				aus.Play();
				startTime = Time.time;
			}
		} else {
			if (aus.volume < baseVolume) {
				if (Time.time - startTime > 1) {
					aus.volume += 0.05f;
					startTime = Time.time;
				}
			}
		}
	}
}
