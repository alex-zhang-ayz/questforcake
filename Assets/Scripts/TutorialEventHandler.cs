using UnityEngine;
using System.Collections;

public class TutorialEventHandler : MonoBehaviour, TriggerEventHandler {

	public GameObject sideButtonController;
	public GameObject dcObject;
	public GameObject pehObject;
	private PrimaryEventHandler peh;
	private DestroyControls dc;
	private SidebuttonBehaviour sbb;

	void Start () {
		peh = pehObject.GetComponent<PrimaryEventHandler> ();
		sbb = sideButtonController.GetComponent<SidebuttonBehaviour> ();
		dc = dcObject.GetComponent<DestroyControls> ();
	}
	
	public void handleEvent(string s){
		switch (s) {
		case "area 2":
			sbb.setButtonActive(1);
			dc.checkOverUI = true;
			dc.destroyMode = true;
			peh.sendText("area2");
			break;
		case "area 3":
			sbb.setButtonActive(2);
			peh.sendText("area3");
			break;
		}
	}

	void Update () {
	
	}
}
