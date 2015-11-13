using UnityEngine;
using System.Collections;

public class PlayerSoundFX : MonoBehaviour {

	public AudioClip[] sounds;
	private AudioSource aus;

	private bool canPlayDelay = false;
	private float startTime = -1, delay = 0.1f;

	void Start () {
		aus = GetComponent<AudioSource> ();
	}

	public void playSound(int i){
		if (i < sounds.Length && canPlayDelay) {
			aus.PlayOneShot (sounds [i]);
			startTime = Time.time;
			canPlayDelay = false;
		} else {
			//Debug.Log("Index out of range of sounds array");
		}
	}

	void Update () {
		if (!canPlayDelay && Time.time - startTime > delay) {
			canPlayDelay = true;
		}
	}
}
