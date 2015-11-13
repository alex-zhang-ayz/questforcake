using UnityEngine;
using System.Collections;

public class NPCBehaviour : MonoBehaviour {

	public GameObject player;
	public GameObject chatHandlerObj;
	public bool hasQuest = false;
	public GameObject qmObject;
	public GameObject questIcon;
	public float minimumVolumeRequirement;
	private int questID;
	private ChatHandler ch;
	private PlayerVolume pv;
	private NPC_Quest npc_quest_manager;
	private Quest quest;
	private QuestManager qm;
	private static int globalQuestID = -1;
	private bool started;


	// Use this for initialization
	void Start () {
		qm = qmObject.GetComponent<QuestManager> ();
		pv = player.GetComponent<PlayerVolume> ();
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
		if (hasQuest) {
			globalQuestID++;
			questID = globalQuestID;
			addtoGQID ();
			questIcon.SetActive (true);
		} else {
			questIcon.SetActive(false);
		}
		started = false;
		npc_quest_manager = this.GetComponent<NPC_Quest> ();
		quest = npc_quest_manager.getQuest ();
	}

	public void toggleQuestIcon(){
		questIcon.SetActive(!questIcon.activeSelf);
	}

	public bool startQuest(){
		if (checkVolumeReq () && !started) {
			started = true;
			npc_quest_manager.startQuest ();
			quest = npc_quest_manager.getQuest();
			qm.addToQuests(questID, quest);
			questIcon.SetActive (false);
			return true;
		}
		return false;
	}

	private bool checkVolumeReq(){
		bool canStart = true;
		if (pv.getCurrScaleVolume () < minimumVolumeRequirement) {
			sendRequirementText();
			canStart = false;
		}
		return canStart;
	}

	private void sendRequirementText(){
		string reqStr = "You need to have at least <color=red>" + minimumVolumeRequirement.ToString()
			+ "</color> volume to start this quest.";
		ChatRequest c = new ChatRequest (reqStr);
		ch.sendRequest (c);
	}

	private static void addtoGQID(){
		globalQuestID += 1;
	}

	public bool getHasQuest(){
		return hasQuest;
	}

	public int getQuestID(){
		return questID;
	}

	public void removeQuest(){
		started = false;
		qm.removeQuest (questID);
		questIcon.SetActive (true);
	}

	public void finishQuest(){
		hasQuest = false;
		qm.finishQuest (questID);
		this.tag = "Destructible";
	}

	void Update () {
	
	}
}
