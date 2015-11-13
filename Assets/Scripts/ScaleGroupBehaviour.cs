using UnityEngine;
using System.Collections;

public class ScaleGroupBehaviour : MonoBehaviour {

	public GameObject npc;
	private NPCBehaviour npcb;
	private float groupVolume = -1;

	private int savedCC = -1;
	private bool started = false;

	void Start () {
		npcb = npc.GetComponent<NPCBehaviour> ();
	}


	public float getGroupVolume(){
		return Mathf.Floor(groupVolume);
	}

	void setGroupVolume(){
		float v = 0;
		foreach (Transform child in transform) {
			DestructibleBehaviour db = child.GetComponent<DestructibleBehaviour>();
			if (db != null){
				v += db.getCurrentVolume();
			}
		}
		groupVolume = v;
	}

	void Update () {
		setGroupVolume ();
		if (!started) {
			if (transform.childCount != savedCC){
				if (transform.childCount < savedCC){
					npcb.startQuest();
					started = false;
				}
				savedCC = transform.childCount;
			}
		}
	}
}
