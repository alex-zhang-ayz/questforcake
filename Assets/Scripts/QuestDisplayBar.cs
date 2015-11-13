using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuestDisplayBar : MonoBehaviour {

	public Quest quest;
	public int childIndex;
	public GameObject QMObj;
	public GameObject hoverObject;
	private QuestManager qm;
	private HoverObjectBehaviour hob;
	private Text text;
	private string hoverText = "";
	private RectTransform rt;
	private bool mode = true; // curr = true, unclaim = false;

	void Start () {
		text = transform.GetChild (0).GetComponent<Text> ();
		qm = QMObj.GetComponent<QuestManager> ();
		hob = hoverObject.GetComponent<HoverObjectBehaviour> ();
		rt = this.GetComponent<RectTransform> ();
		this.GetComponent<Button>().onClick.AddListener(delegate() {sendToManager();});
	}

	private void sendToManager(){
		qm.claimQuest (childIndex);
	}

	private void checkOver(){
		Vector3[] corners  = new Vector3[4];
		rt.GetWorldCorners(corners);
		Vector3 mPos = Input.mousePosition;
		if (mPos.x >= corners[0].x && mPos.x < corners [2].x 
			&& mPos.y >= corners [0].y && mPos.y < corners [2].y) {
			hob.setOn (hoverText, gameObject.GetHashCode());
		} else{
			hob.setOff(gameObject.GetHashCode());
		}


	}

	public void setMode(bool b){
		mode = b;
	}

	public void toggleMode(){
		mode = !mode;
	}

	private void buttonBehaviour(){
		if (mode) {
			Button b = this.GetComponent<Button>();
			if (b != null && b.enabled){
				b.enabled = false;
			}
		} else {
			Button b = this.GetComponent<Button>();
			if (b != null && !b.enabled){
				b.enabled = true;
			}
		}
	}

	void Update () {
		if (mode) {
			checkOver ();
		}
		buttonBehaviour();
		if (quest != null) {
			text.text = quest.getName();
			hoverText = quest.getDescription();
		} else {
			text.text = "";
			hoverText = "";
		}
	}
}
