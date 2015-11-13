using UnityEngine;
using System.Collections;

public class QuestStartObjects : MonoBehaviour {

	private GameObject parentNPC;
	private NPCBehaviour npcB;

	void Start () {
		Transform t = ExtraFunctions.searchInAncestor (this.transform, typeof(NPCBehaviour));
		if (t != null) {
			parentNPC = t.gameObject;
			npcB = parentNPC.GetComponent<NPCBehaviour> ();
		}

	}

	public void startAssociatedQuest(){
		if (npcB != null) {
			npcB.startQuest ();
		} else {
			print ("no NPCBehaviour found");
		}
	}


	void Update () {
	
	}
}
