using UnityEngine;
using System.Collections;

public class NPC_DestroyObjects : MonoBehaviour, NPC_Quest{

	public GameObject player;
	public TextAsset startQuestText;
	public TextAsset finishQuestText;
	public GameObject chatHandlerObject;
	public int rewardLevels = 1;
	public GameObject objcHandlerObj;
	public string quest_name = "";
	public string quest_description = "";
	private PlayerVolume pv;
	private ObjectiveHandler oh;
	private ChatHandler ch;
	private string sQstring;
	private string fQstring;
	private string[] sQLines;
	private string[] fQLines;
	private NPCBehaviour npcb;
	public GameObject questObjectContainer;
	public int levelReward = 1;
	private Transform objectContainer;
	private bool questStarted;
	private bool questFinished;
	private DestructiblesHandler rewardHandler;
	private Quest q;
	private Reward r;


	void Start () {
		oh = objcHandlerObj.GetComponent<ObjectiveHandler> ();
		pv = player.GetComponent<PlayerVolume> ();
		sQstring = startQuestText.text;
		fQstring = finishQuestText.text;
		char[] delimitingChars = {'\n'};
		sQLines = sQstring.Split (delimitingChars);
		fQLines = fQstring.Split (delimitingChars);
		ch = chatHandlerObject.GetComponent<ChatHandler> ();
		npcb = this.GetComponent<NPCBehaviour> ();
		objectContainer = questObjectContainer.transform;
		questStarted = false;
		questFinished = false;
		r = new Reward (0, levelReward);
		q = new Quest (this.gameObject, r, quest_name, quest_description);
	}
	public Quest getQuest(){
		return q;
	}

	public void startQuest(){
		foreach (string s in sQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		questStarted = true;
		objectContainer.GetComponent<DestructiblesHandler> ().canDestroy = true;
		if (!oh.isAnObjective (this.gameObject)) {
			oh.addObjective (this.gameObject);
		}
		oh.addObjective (transform.GetChild (0).gameObject);
	}

	private void finishQuest(){
		foreach (string s in fQLines) {
			ChatRequest c = new ChatRequest(s);
			ch.sendRequest(c);
		}
		questFinished = true;
		oh.removeObjective (transform.GetChild (0).gameObject);
		if (oh.isAnObjective (this.gameObject)) {
			oh.removeObjective (this.gameObject);
		}

	}

	void Update () {
		if (questStarted && objectContainer.childCount == 0 && !questFinished) {
			finishQuest();
			npcb.finishQuest();
		}
	}
}
