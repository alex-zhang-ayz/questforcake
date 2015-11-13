using UnityEngine;
using System.IO;
using System.Collections;

public class WaterAbsorb : MonoBehaviour, QuestTrigger{

	public GameObject wellObject;
	public GameObject targetYObj;
	public GameObject overflowObj;
	public GameObject waterAniObj;
	public GameObject npc;
	private NPC_TriggerQuest ntq;
	public string filename;
	private float targetY;
	private float innerRadius;
	private float addedVolume;
	private float baseY;
	private float fudge = 2;
	public int finished = 0; //0: in progress, 1: failed, 2: done

	private string extraLines = "";

	void Start () {
		ntq = npc.GetComponent<NPC_TriggerQuest> ();

		baseY = transform.position.y;
		targetY = targetYObj.transform.position.y;


		float topY = transform.position.y + transform.localScale.z / 2;

		innerRadius = wellObject.transform.localScale.x * 0.75f;

		string s_innerRadius = "" + innerRadius;
		string maxHeight = "" + (targetY + fudge - topY);
		string minHeight = "" + (targetY - fudge - topY);

		saveExtraLines (s_innerRadius, maxHeight, minHeight);

	}

	void saveExtraLines(string inRad, string maxH, string minH){
		string s = string.Format ("<color=blue>Well inner Radius</color>: {0}\n<color=orange>Needed height range:</color> {1} - {2}",
		                         inRad, minH, maxH);
		extraLines = s;
	}

	public string getExtraLines(){
		return extraLines;
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Player") {
			PlayerVolume pv = c.GetComponent<PlayerVolume>();
			if (pv != null){
				addedVolume = pv.getCurrentVolume();
			}
		}
	}

	void OnTriggerExit(Collider c){
		if (c.gameObject.tag == "Player") {
			addedVolume = 0;
		}
	}

	void checkHeight(){
		if (finished == 0) {
			float height = Mathf.Round (addedVolume / (Mathf.PI * innerRadius * innerRadius));
			Vector3 newPos = transform.position;
			newPos.y = baseY + height;
			float topY = newPos.y + transform.localScale.z / 2;

			if (topY >= targetY - fudge && topY <= targetY + fudge) {
				waterAniObj.SetActive(true);
				finished = 2;
			} else if (topY > targetY + fudge) {
				newPos.y = wellObject.transform.position.y + wellObject.transform.localScale.z / 2;
				finished = 1;
			} else {
				finished = 0;
			}
			transform.position = newPos;
		}
	}
	public int getFinishValue(){
		return finished;
	}

	void checkOverflow(){
		if (finished == 1) {
			overflowObj.SetActive (true);
		} else {
			overflowObj.SetActive(false);
		}
	}

	public void reset(){
		addedVolume = 0;
		finished = 0;
	}

	void Update () {
		if (ntq.getFinishValue () >= 0) {
			checkHeight ();
			checkOverflow ();
		}
	}
}
