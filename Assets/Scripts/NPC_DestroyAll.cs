using UnityEngine;
using System.Collections;

public class NPC_DestroyAll : MonoBehaviour, NPC_Quest {

	public TextAsset startQuestText;
	public TextAsset failedQuestText;
	public TextAsset finishQuestText;
	public GameObject spawnerObj;
	public GameObject chatHandlerObj;
	public GameObject rewardParent;
	public GameObject objcHandlerObj;
	public int rewardLevels = 1;
	public string quest_name = "";
	public string quest_description = "";

	private ObjectiveHandler oh;
	private ChatHandler ch;
	private string sQstring;
	private string fQstring;
	private string fAQstring;
	private string[] sQLines;
	private string[] fQLines;
	private string[] fAQLines;
	private NPCBehaviour npcb;
	private DestructiblesHandler rewardHandler;
	private bool questStarted;
	private bool questFinished;
	private Spawner spawner;

	private Quest q;
	private Reward r;

	private bool startCheckChat;

	void Start () {
		startCheckChat = false;

		spawner = spawnerObj.GetComponent<Spawner> ();
		oh = objcHandlerObj.GetComponent<ObjectiveHandler> ();
		sQstring = startQuestText.text;
		fAQstring = failedQuestText.text;
		fQstring = finishQuestText.text;
		char[] delimitingChars = {'\n'};
		sQLines = sQstring.Split (delimitingChars);
		fQLines = fQstring.Split (delimitingChars);
		fAQLines = fAQstring.Split (delimitingChars);
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
		npcb = this.GetComponent<NPCBehaviour> ();
		rewardHandler = rewardParent.GetComponent<DestructiblesHandler> ();
		questStarted = false;
		questFinished = false;

		r = new Reward (0, rewardLevels);
		q = new Quest (this.gameObject, r, quest_name, quest_description);
	}

	public void startQuest(){
		foreach (string s in sQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		questStarted = true;
		if (!oh.isAnObjective (this.gameObject)) {
			oh.addObjective (this.gameObject);
		}
		oh.addObjective (rewardParent);

		startCheckChat = true;

	}

	public Quest getQuest(){
		return q;
	}

	private void finishQuest(){
		foreach (string s in fQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		questFinished = true;
		oh.removeObjective (rewardParent);
		if (oh.isAnObjective (this.gameObject)) {
			oh.removeObjective (this.gameObject);
		}
		
	}

	private void failedQuest(){
		foreach (string s in fAQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		//npcb.toggleQuestIcon ();
		questStarted = false;
		questFinished = false;
		oh.removeObjective (rewardParent);
		if (oh.isAnObjective (this.gameObject)) {
			oh.removeObjective (this.gameObject);
		}
	}

	private void unlockReward(){
		rewardHandler.canDestroy = true;
	}

	void Update () {
		if (startCheckChat) {
			if (!ch.isInChat()){
				spawner.startSpawn ();
				startCheckChat = false;
			}
		}

		if (questStarted && spawner.getFinish() && !questFinished) {
			if (spawner.getFailed()){
				failedQuest();
				spawner.resetAll();
				npcb.removeQuest();
			}else{
				finishQuest();
				npcb.finishQuest();
				unlockReward();
			}
		}
	}
}
