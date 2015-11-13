using UnityEngine;
using System.Collections;

public class ChatRequest{

	public string text;
	public int chatType;

	public ChatRequest(string s){
		text = s;
		chatType = s.GetHashCode ();
	}
	public override string ToString(){
		return text;
	}
	public override bool Equals(System.Object obj){
		ChatRequest ch = obj as ChatRequest;
		if ((object)ch == null) {
			return false;
		}
		return text == ch.text;
	}
}
