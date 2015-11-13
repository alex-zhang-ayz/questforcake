using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrimaryEventHandler : MonoBehaviour {

	public TextAsset eventLinesText;
	public GameObject chatHandlerObj;
	private ChatHandler ch;
	private string eventLinesString;
	private Dictionary<string, string> linesDictionary;

	void Start () {
		ch = chatHandlerObj.GetComponent<ChatHandler> ();

		linesDictionary = new Dictionary<string, string> ();
		char[] firstSplitChars = {'\n'};
		char[] secondSplitChars = {':'};
		eventLinesString = eventLinesText.text;
		string[] lines = eventLinesString.Split (firstSplitChars);
		foreach (string s in lines) {
			string[] things = s.Split(secondSplitChars);
			if (things.Length == 2){
				linesDictionary.Add(things[0], things[1]);
			}
		}
	}

	public void sendText(string s){
		if (linesDictionary.ContainsKey (s)) {
			string l = linesDictionary [s];
			ChatRequest c = new ChatRequest (l);
			ch.sendRequest (c);
		} else {
			Debug.LogError ("Key does not exist");
		}
	}

	void Update () {
	
	}
}
