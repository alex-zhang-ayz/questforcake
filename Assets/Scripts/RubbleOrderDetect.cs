using UnityEngine;
using System.Collections;

public class RubbleOrderDetect : MonoBehaviour {

	public bool hasNext = true;
	public GameObject nextInOrder;
	public GameObject triggerObject;
	private RubbleQuestTrigger rqt;

	void Start () {
		rqt = triggerObject.GetComponent<RubbleQuestTrigger> ();
	}
	

	void Update () {
		if (hasNext && nextInOrder == null) {
			rqt.setFailed();
		}
	}
}
