using UnityEngine;
using System.Collections;

public class Level2_ShowComic : MonoBehaviour {

	public float delayBetweenPanels = 2f;
	private int childIndex, maxIndex;
	private float startTime;
	private bool finished = false;

	void Start () {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}
		childIndex = 0;
		maxIndex = transform.childCount - 1;
		startTime = Time.time;
	}

	public bool getFinished(){
		return finished;
	}

	void Update () {
		if ((Time.time - startTime > delayBetweenPanels)) {
			if (childIndex > maxIndex){
				finished = true;
			}else{
				transform.GetChild(childIndex).gameObject.SetActive(true);
				childIndex++;
				startTime = Time.time;
			}
		}
	}
}
