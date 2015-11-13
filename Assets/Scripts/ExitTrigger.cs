using UnityEngine;
using System.Collections;

public class ExitTrigger : MonoBehaviour {

	public GameObject player;
	public GameObject musicObj;
	public string nextLevelName = "";
	public bool saveData = true;
	private AudioSource aus;

	void Start () {
		aus = musicObj.GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "Player") {
			if (saveData){
				player.GetComponent<PlayerVolume>().saveData();
			}
			for (int i=9;i>0;i--){
				aus.volume = i * 0.1f;
				StartCoroutine(waitfortime());
			}
			Application.LoadLevel(nextLevelName);
		}
	}

	IEnumerator waitfortime(){
		yield return new WaitForSeconds(0.5f);
	}

	void Update () {
	
	}
}
