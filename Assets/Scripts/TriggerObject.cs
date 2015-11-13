using UnityEngine;
using System.Collections;

public class TriggerObject : MonoBehaviour {

	public GameObject triggerHandler;
	public TriggerEventHandler teh;
	public string triggerString = "";
	public bool triggerOnce = true;
	private bool triggered = false;

	void Start () {	
		teh = triggerHandler.GetComponent<TriggerEventHandler> ();
	}

	void OnTriggerExit(Collider c){
		if (c.gameObject.tag == "Player" && triggerOnce && !triggered) {
			sendEvent();
			triggered = true;
		}
	}

	private void sendEvent(){
		teh.handleEvent (triggerString);
	}

	void Update () {
	
	}
}
