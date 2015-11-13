using UnityEngine;
using System.Collections;

public class GlowController : MonoBehaviour {

	//For the unclaimed quests

	public GameObject questMangagerObject;
	private QuestManager qm;
	private GameObject glow;
	private float startTime;
	private float interval = 1;

	void Start () {
		qm = questMangagerObject.GetComponent<QuestManager> ();
		glow = transform.GetChild (0).gameObject;
		startTime = Time.time;
	}

	void Update () {
		if (qm.unclaimedIsEmpty ()) {
			glow.SetActive (false);
		} else {
			if (Time.time - startTime > interval) {
				glow.SetActive (!glow.activeSelf);
				startTime = Time.time;
			}
		}
	}
}
