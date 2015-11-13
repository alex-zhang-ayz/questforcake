using UnityEngine;
using System.Collections;

public class ScalesQuestTrigger : MonoBehaviour, QuestTrigger {

	public GameObject npc;
	public GameObject balanceArmObject;
	public GameObject[] objectContainers;
	public GameObject[] respawnObjects;
	private NPC_TriggerQuest ntq;
	private BalanceArmBehaviour bab;
	private int finishValue = 0;

	void Start () {
		bab = balanceArmObject.GetComponent<BalanceArmBehaviour> ();
		ntq = npc.GetComponent<NPC_TriggerQuest> ();
	}

	public void reset(){
		foreach (GameObject g in respawnObjects) {
			RespawnableObjects ro = g.GetComponent<RespawnableObjects> ();
			ro.removeContainerObjects();
			ro.copyObjects();
		}
		finishValue = 0;

	}
	public int getFinishValue(){
		return finishValue;
	}
	public string getExtraLines(){
		return "";
	}

	void checkFinish(){
		if (ntq.getFinishValue () >= 0) {
			if (bab.isBalanced) {
				finishValue = 2;
			}
			int total = 0;
			foreach (GameObject g in objectContainers){
				total += g.transform.childCount;
			}
			if (total == 0){
				finishValue = 1;
			}
		}
	}

	void Update () {
		checkFinish ();
	}
}
