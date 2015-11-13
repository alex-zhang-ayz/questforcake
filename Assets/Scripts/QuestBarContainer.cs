using UnityEngine;
using System.Collections;

public class QuestBarContainer : MonoBehaviour {

	void Start () {

	}

	public void toggleAll(){
		foreach (Transform child in transform) {
			QuestDisplayBar qdb = child.GetComponent<QuestDisplayBar>();
			if (qdb != null){
				qdb.toggleMode();
			}
		}
	}

	void Update () {
	
	}
}
