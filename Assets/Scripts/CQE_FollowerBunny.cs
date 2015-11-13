using UnityEngine;
using System.Collections;

public class CQE_FollowerBunny : MonoBehaviour, CompleteQuestEvent {

	public TextAsset initialFollowerText;
	public GameObject chatHandlerObj;
	public GameObject npcStandIn;
	public GameObject waterAniObj;
	public GameObject bunny;

	private ChatHandler ch;
	private string ftString;
	private string[] ftLines;

	private WaterAnimation wani;
	private bool questComplete = false;

	private bool doOnce = false;

	void Start () {
		ftString = initialFollowerText.text;
		char[] delimitingChars = {'\n'};
		ftLines = ftString.Split (delimitingChars);
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
		wani = waterAniObj.GetComponent<WaterAnimation> ();
	}

	void finish(){
		Destroy (npcStandIn);
		bunny.SetActive (true);
	}

	public void complete(){
		questComplete = true;
	}

	void Update () {
		if (!doOnce && questComplete && wani.finished){
			foreach (string s in ftLines) {
				ChatRequest c = new ChatRequest(s);
				ch.sendRequest(c);
			}

			finish ();
			doOnce = true;
		}
	}
}
