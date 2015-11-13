using UnityEngine;
using System.Collections;

public class StartQuestObject : MonoBehaviour {

	public GameObject npc;
	public GameObject targetObjects;
	private NPCBehaviour nb;

	void Start () {
		nb = npc.GetComponent<NPCBehaviour> ();
	}

	void OnTriggerEnter(Collider c){
		if (c.tag == "Player") {
			if (nb.startQuest()){
				targetObjects.GetComponent<DestructiblesHandler>().canDestroy = true;
			}
		}
	}

	void Update () {
	
	}
}
