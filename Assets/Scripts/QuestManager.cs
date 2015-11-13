using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour {

	public GameObject questDisplay;
	public GameObject menuControlObject;
	private MenuControl mc;
	private Dictionary<int, Quest> quests;
	private List<Quest> unclaimed_quests;
	private bool questDisplayMode; //true = current, false = unclaimed
	
	void Start () {
		questDisplayMode = true;
		mc = menuControlObject.GetComponent<MenuControl> ();
		quests = new Dictionary<int, Quest> ();
		unclaimed_quests = new List<Quest> ();
	}

	public void addToQuests(int i, Quest q){
		print (q.ToString());
		quests.Add (i, q);
		if (mc != null) {
			mc.openPanel(2);
		}
	}

	public void removeQuest(int i){
		quests.Remove (i);
	}

	public void finishQuest(int i){
		if (quests.ContainsKey (i)) {
			print (quests [i].ToString ());
			unclaimed_quests.Add (quests [i]);
			quests.Remove (i);
		} else {
			Debug.Log("Key not found");
		}
	}

	void Awake(){
		DontDestroyOnLoad (this);
	}

	public void setDisplayMode(bool b){
		questDisplayMode = b;
	}

	public void toggleDisplayMode(){
		questDisplayMode = !questDisplayMode;
	}

	public bool unclaimedIsEmpty(){
		return unclaimed_quests.Count == 0;
	}

	private void showCurrentQuests(){
		int index = 0;
		foreach (Quest q in quests.Values) {
			Transform child = questDisplay.transform.GetChild(index);
			child.gameObject.SetActive(true);
			QuestDisplayBar qdb = child.GetComponent<QuestDisplayBar>();
			qdb.quest = q;
			qdb.setMode(true);
			index++;
		}
		for (int i=index; i<questDisplay.transform.childCount; i++) {
			questDisplay.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	private void showUnclaimedQuests(){
		int index = 0;
		foreach (Quest q in unclaimed_quests) {
			Transform child = questDisplay.transform.GetChild(index);
			child.gameObject.SetActive(true);
			QuestDisplayBar qdb = child.GetComponent<QuestDisplayBar>();
			qdb.childIndex = index;
			qdb.quest = q;
			qdb.setMode(false);
			index++;
		}
		for (int i=index; i<questDisplay.transform.childCount; i++) {
			questDisplay.transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	public void claimQuest(int i){
		unclaimed_quests [i].claimReward ();
		unclaimed_quests.Remove (unclaimed_quests [i]);
	}

	void Update () {
		if (questDisplay.activeSelf) {
			if (questDisplayMode) {
				showCurrentQuests ();
			} else {
				showUnclaimedQuests ();
			}
		}
	}
}
