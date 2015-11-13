using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChatHandler : MonoBehaviour {

	public GameObject destroyControllerObject;
	private DestroyControls dc;
	private Text chatText;
	public GameObject chatPanel;
	private ArrayList chatRequests;
	private Dictionary<int, ChatRequest> chatCollisionDetect;
	private ChatRequest currentRequest;
	private bool prevMouseDown;
	private bool nowfinished;
	private List<ChatRequest> chatLog;
	private bool resetDC = false;

	// Use this for initialization
	void Start () {
		chatCollisionDetect = new Dictionary<int, ChatRequest> ();
		chatLog = new List<ChatRequest> ();
		dc = destroyControllerObject.GetComponent<DestroyControls> ();
		chatText = chatPanel.transform.GetChild (0).GetComponent<Text> ();
		chatRequests = new ArrayList ();
		currentRequest = null;
		chatPanel.SetActive (false);
		prevMouseDown = true;
		nowfinished = false;
	}

	public void sendRequest(ChatRequest c){
		if (!chatCollisionDetect.ContainsKey (c.chatType)) {
			chatRequests.Add (c);
			chatCollisionDetect.Add(c.chatType, c);
			if (chatRequests.Count == 1) {
				currentRequest = c;
			}
		}

	}

	private void addToChatLog(ChatRequest cr){
		if (!chatLog.Contains (cr)) {
			chatLog.Add (cr);
		}
	}

	public List<ChatRequest> getChatLog(){
		return chatLog;
	}

	public void completeRequest(){
		if (chatRequests.Count > 0) {
			addToChatLog(currentRequest);
			chatCollisionDetect.Remove(((ChatRequest)chatRequests[0]).chatType);
			chatRequests.RemoveAt(0);
			if (chatRequests.Count > 0){
				currentRequest = (ChatRequest)chatRequests[0];
			}else{
				currentRequest = null;
				nowfinished = true;
			}
		}
	}

	private void updateChatspace(){
		if (currentRequest != null) {
			if (dc.destroyMode){
				resetDC = true;
				dc.destroyMode = false;
			}
			chatPanel.SetActive(true);
			chatText.text = currentRequest.text;
		} else {
			if (nowfinished){
				if (resetDC){
					dc.destroyMode = true;
					resetDC = false;
				}
				chatPanel.SetActive(false);
				nowfinished = false;
			}
		}
	}

	public bool isInChat(){
		return currentRequest != null;
	}
	
	void Update () {
		updateChatspace ();
		if (currentRequest != null) {
			bool overUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject ();
			if (!prevMouseDown && Input.GetMouseButtonDown (0) && overUI) {
				completeRequest ();
			}
			prevMouseDown = Input.GetMouseButtonDown (0);
		}
	}
}
