using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveHandler : MonoBehaviour {
	
	private List<GameObject> objectives;
	private List<GameObject> prevObjectives;
	private GameObject arrow;
	private ArrowBehaviour ab;
	private bool showArrow;

	void Start () {
		showArrow = false;
		objectives = new List<GameObject> ();
		prevObjectives = new List<GameObject>();
		arrow = transform.GetChild (0).gameObject;
		ab = arrow.GetComponent<ArrowBehaviour> ();
	}

	public void addObjective(GameObject g){
		objectives.Add (g);
	}

	public void removeObjective(GameObject g){
		prevObjectives.Add (g);
		objectives.Remove (g);
	}

	public bool wasAnObjective(GameObject g){
		return prevObjectives.Contains (g);
	}

	public bool isAnObjective(GameObject g){
		return objectives.Contains (g);
	}

	public void toggleArrow(){
		showArrow = !showArrow;
	}

	void Update () {
		if (objectives.Count > 0 && showArrow) {
			ab.gameObject.SetActive (true);
			ab.setTarget (objectives [objectives.Count - 1]);
		} else {
			ab.gameObject.SetActive(false);
		}
	}
}
