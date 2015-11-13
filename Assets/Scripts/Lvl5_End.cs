using UnityEngine;
using System.Collections;

public class Lvl5_End : MonoBehaviour {
	
	public string loseLevelName = "";
	public string winLevelName = "";
	public GameObject moon;
	private FinalBossBehaviour fbb;

	void Start () {
		fbb = moon.GetComponent<FinalBossBehaviour> ();
	}
	
	void checkLose(){
		if (moon != null) {
			if (fbb.finished) {
				Application.LoadLevel (loseLevelName);
			}
		}
	}

	void checkWin(){
		if (moon == null) {
			Application.LoadLevel(winLevelName);
		}
	}

	void Update () {
		checkWin ();
		checkLose ();
	}
}
