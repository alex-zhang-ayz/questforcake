using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChatlogEntry : MonoBehaviour {

	public GameObject hoverObject;
	public GameObject scrollView;
	private Text entryText;
	private HoverObjectBehaviour hob;
	private ChatRequest chatRequest;
	private RectTransform rt;

	// Use this for initialization
	void Start () {
		rt = this.GetComponent<RectTransform> ();
		hob = hoverObject.GetComponent<HoverObjectBehaviour> ();
		entryText = transform.GetChild (0).GetComponent<Text> ();
	}

	private bool withinScrollView(){
		RectTransform scrollViewRT = scrollView.GetComponent<RectTransform> ();
		Vector3[] corners = new Vector3[4];
		scrollViewRT.GetWorldCorners (corners);
		Vector3 mPos = Input.mousePosition;
		return mPos.x >= corners [0].x && mPos.x < corners [2].x 
			&& mPos.y >= corners [0].y && mPos.y < corners [2].y;
	}

	private void checkOver(){
		if (chatRequest != null) {
			Vector3[] corners = new Vector3[4];
			rt.GetWorldCorners (corners);
			Vector3 mPos = Input.mousePosition;
			if (withinScrollView() && mPos.x >= corners [0].x && mPos.x < corners [2].x 
				&& mPos.y >= corners [0].y && mPos.y < corners [2].y) {
				hob.setOn (chatRequest.text, gameObject.GetHashCode());
			} else {
				hob.setOff (gameObject.GetHashCode());
			}
		}
		
		
	}

	private void updateText(){
		if (entryText != null && chatRequest != null) {
			entryText.text = chatRequest.text;
		}
	}

	public void setChatRequest(ChatRequest cr){
		chatRequest = cr;
	}
	
	// Update is called once per frame
	void Update () {
		checkOver ();
		updateText ();
	}
}
