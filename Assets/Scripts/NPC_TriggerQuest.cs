using UnityEngine;
using System.Collections;

public class NPC_TriggerQuest : MonoBehaviour, NPC_Quest {

	public TextAsset startQuestText;
	public TextAsset failedQuestText;
	public TextAsset finishQuestText;
	public int rewardLevels = 5;
	public string quest_name = "";
	public string quest_description = "";
	public GameObject chatHandlerObj;
	public GameObject questTriggerObj;
	public GameObject completeQuestEventObj;
	private CompleteQuestEvent cqe;
	private QuestTrigger trigger;
	private NPCBehaviour npcb;
	private ChatHandler ch;

	private string sQstring;
	private string fQstring;
	private string fAQstring;
	private string extraString;
	private string[] extraLinesArray;
	private string[] sQLines;
	private string[] fQLines;
	private string[] fAQLines;

	public bool hasExtraLines = false;

	private Quest q;
	private Reward r;

	private bool started = false;
	private int finishValue = -1; //0: in progress, 1: failed, 2: finished
	private bool complete = false;

	void Start () {
		if (completeQuestEventObj != null) {
			cqe = completeQuestEventObj.GetComponent<CompleteQuestEvent> ();
		}
		trigger = questTriggerObj.GetComponent<QuestTrigger> ();
		sQstring = startQuestText.text;
		fAQstring = failedQuestText.text;
		fQstring = finishQuestText.text;
		char[] delimitingChars = {'\n'};
		sQLines = sQstring.Split (delimitingChars);
		fQLines = fQstring.Split (delimitingChars);
		fAQLines = fAQstring.Split (delimitingChars);
		npcb = this.GetComponent<NPCBehaviour> ();

		r = new Reward (0, rewardLevels);
		q = new Quest (this.gameObject, r, quest_name, quest_description);
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
	}

	public void startQuest(){
		foreach (string s in sQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		started = true;
		finishValue = 0;
		complete = false;
		sendExtraLines ();
	}
	public Quest getQuest(){
		return q;
	}

	void sendExtraLines(){
		if (hasExtraLines) {
			ChatRequest c = new ChatRequest(trigger.getExtraLines());
			ch.sendRequest(c);
		}

	}

	public int getFinishValue(){
		return finishValue;
	}

	void failedQuest(){
		foreach (string s in fAQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		npcb.removeQuest ();
		complete = true;
		started = false;
		trigger.reset ();
	}

	void completeQuest(){
		foreach (string s in fQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		npcb.finishQuest ();
		if (cqe != null) {
			cqe.complete ();
		}
		complete = true;
	}

	void checkFinished(){
		finishValue = trigger.getFinishValue ();
		if (finishValue == 1) {
			failedQuest ();
		} else if (finishValue == 2) {
			completeQuest();
		}
	}

	void Update () {
		if (!complete && started) {
			checkFinished ();
		}
	}
}
