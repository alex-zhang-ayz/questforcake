using UnityEngine;
using System.Collections;

public class ShowComic : MonoBehaviour {

	public GameObject musicObj;
	private AudioSource aus;
	public float delayBetweenPanels = 2.25f;
	public string nextLevelName;
	private int childIndex, maxIndex;
	private float startTime;
	public bool reduceVolume = true;

	public bool gotoNext = true;
	private bool finished = false;

	void Start () {
		aus = musicObj.GetComponent<AudioSource> ();
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}
		childIndex = 0;
		maxIndex = transform.childCount - 1;
		startTime = Time.time;
	}

	void gotoNextLevel(){
		Application.LoadLevel (nextLevelName);
	}

	public bool getFinished(){
		return finished;
	}

	void Update () {
		if ((Time.time - startTime > delayBetweenPanels)) {
			if (childIndex > maxIndex){
				if (gotoNext){
					gotoNextLevel();
				}
				finished = true;
			}else{
				transform.GetChild(childIndex).gameObject.SetActive(true);
				childIndex++;
				startTime = Time.time;
				if (reduceVolume){
					aus.volume *= 0.75f;
				}
			}
		}
	}
}
