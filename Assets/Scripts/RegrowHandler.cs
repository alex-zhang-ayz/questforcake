using UnityEngine;
using System.Collections;

public class RegrowHandler : MonoBehaviour {


	public GameObject respawn;
	private RespawnableObjects ro;
	private float startTime = -1;
	private float delay = 5;

	void Start () {
		ro = respawn.GetComponent<RespawnableObjects> ();
	}

	void Update () {
		if (startTime > 0 && Time.time - startTime > delay) {
			ro.removeContainerObjects();
			ro.copyObjects();
			startTime  = -1;
		}

		if (transform.childCount != ro.getBaseChildCount () && startTime < 0) {
			startTime = Time.time;
		}
	}
}
