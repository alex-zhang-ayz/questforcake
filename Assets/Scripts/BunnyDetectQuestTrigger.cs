using UnityEngine;
using System.Collections;

public class BunnyDetectQuestTrigger : MonoBehaviour, QuestTrigger {

	public GameObject bunny;
	public GameObject snorlaxVolumeObj;
	public float scareDistance = 1000;
	private int finished = 0;

	void Start () {
		
	}

	public int getFinishValue(){
		return finished;
	}

	public string getExtraLines(){
		return "";
	}

	public void reset(){}

	bool sizeWorks(){
		DestructibleBehaviour snDB = snorlaxVolumeObj.GetComponent < DestructibleBehaviour >();
		FollowerVolumeController fvc = bunny.GetComponent<FollowerVolumeController> ();
		//print ("Snorlax Volume: " + snDB.getCurrentVolume ());
		//print ("Bunny Volume: " + fvc.getCurrentVolume ());

		return fvc.getCurrentVolume () > 0.025 * snDB.getCurrentVolume ();
	}

	void searchForBunny(){
		if (finished == 0) {
			//(bunny.transform.position - transform.position).magnitude < scareDistance && 
			if (sizeWorks ()) {
				print ("unlimited bunny works");
				finished = 2;
			}
		}

	}

	void Update () {
		searchForBunny ();
	}
}
