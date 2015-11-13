using UnityEngine;
using System.Collections;

public class CQE_Scales : MonoBehaviour, CompleteQuestEvent {

	public GameObject blockade;

	void Start () {
	
	}
	
	public void complete(){
		blockade.SetActive (false);
	}


	void Update () {
	
	}
}
