using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatlogContainer : MonoBehaviour {

	public GameObject chatlogEntryPrefab;
	public GameObject chatHandlerObj;
	public GameObject entryContainer;
	public GameObject hoverObject;
	public GameObject scrollView;
	private RectTransform entryRectTrans;
	private RectTransform containerRectTrans;
	private float startYPos = 191;
	private float spacing = 4;
	private ChatHandler ch;
	private List<ChatRequest> chChatlog;
	private int currentChatElements = 0;

	void Start () {
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
		entryRectTrans = chatlogEntryPrefab.GetComponent<RectTransform> ();
		containerRectTrans = this.GetComponent<RectTransform> ();
	}
	
	private void checkContainerSize(){
		float entriesSize = (entryRectTrans.rect.height + spacing) * (chChatlog.Count + 1);
		if (entriesSize >= containerRectTrans.rect.height){
			float newHeight = Mathf.Round(entriesSize / 100) * 100 + 100;
			print (newHeight);
			containerRectTrans.sizeDelta = new Vector2 (containerRectTrans.rect.width, newHeight);
		}
	}

	private void updateChatEntries(){
		chChatlog = ch.getChatLog ();
		if (chChatlog.Count != currentChatElements) {
			foreach(Transform child in entryContainer.transform){
				Destroy (child.gameObject);
			}
			print ("chatlog differs");
			currentChatElements = chChatlog.Count;
			checkContainerSize ();
			float ypos = this.startYPos;
			foreach(ChatRequest cr in chChatlog){
				GameObject entry = GameObject.Instantiate(chatlogEntryPrefab) as GameObject;
				entry.transform.SetParent(entryContainer.transform);
				entry.transform.localScale = Vector3.one;
				entry.transform.localPosition = new Vector3 (0, ypos, 0);
				ChatlogEntry cle = entry.GetComponent<ChatlogEntry>();
				cle.setChatRequest(cr);
				cle.hoverObject = hoverObject;
				cle.scrollView = scrollView;
				ypos -= entryRectTrans.rect.height + spacing;
			}

		}
	}

	void Update () {
		updateChatEntries ();

	}
}
