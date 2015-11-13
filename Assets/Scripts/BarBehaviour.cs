using UnityEngine;
using System.Collections;

public class BarBehaviour : MonoBehaviour {

	public GameObject primaryEventObj;
	public GameObject objcHandlerObj;
	public GameObject nextObjective;
	private ObjectiveHandler oh;
	private PrimaryEventHandler peh;
	private static bool hasCollided = false;
	private DestructibleBehaviour db;

	void Start () {
		db = this.GetComponent<DestructibleBehaviour> ();
		peh = primaryEventObj.GetComponent<PrimaryEventHandler> ();
		oh = objcHandlerObj.GetComponent<ObjectiveHandler> ();
	}

	public void sendText(){
		peh.sendText("barCollide");
		if (!oh.wasAnObjective (nextObjective) && !oh.isAnObjective(nextObjective)) {
			oh.addObjective (nextObjective);
		}
		hasCollided = true;
	}

	public bool getHasCollided(){
		return hasCollided;
	}

	void OnCollisionEnter(Collision c){
		if (c.collider.tag == "Player") {
			PlayerVolume pv = c.collider.GetComponent<PlayerVolume>();
			float playerVolume = pv.getCurrScaleVolume();
			float barVolume = db.getCurrentVolume();
			if (playerVolume < barVolume && !hasCollided){
				sendText();
			}
		}
	}
	

	void Update () {
	
	}
}
