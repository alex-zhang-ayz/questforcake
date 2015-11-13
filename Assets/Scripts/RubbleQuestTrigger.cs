using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RubbleQuestTrigger : MonoBehaviour, QuestTrigger{

	public GameObject rubbleParent;
	public GameObject npc;

	public GameObject failedView;
	public GameObject bunnies;

	private NPC_TriggerQuest ntq;
	private int finishValue = 0;
	private int savedChildCount;

	void Start () {
		ntq = npc.GetComponent<NPC_TriggerQuest> ();
		savedChildCount = rubbleParent.transform.childCount;
	}

	public void reset(){

	}
	public int getFinishValue(){
		return finishValue;
	}
	public string getExtraLines(){
		return "";
	}

	public void setFailed(){
		setFailedView ();
		bunnies.SetActive (false);
		rubbleParent.SetActive (false);
		finishValue = 1;
	}

	void setFailedView(){
		failedView.SetActive (true);
		foreach (Transform child in failedView.transform) {
			foreach (Transform rChild in rubbleParent.transform){
				if (child.name == rChild.name){
					child.gameObject.SetActive(true);
				}
			}
		}
	}

	void checkFinish(){
		if (finishValue == 0 && rubbleParent.transform.childCount != savedChildCount) {
			if (rubbleParent.transform.childCount == 0){
				finishValue = 2;
			}
			savedChildCount = rubbleParent.transform.childCount;
		}

	}

	void Update () {
		if (ntq.getFinishValue () >= 0) {
			checkFinish();
		}
	}
}
