using UnityEngine;
using System.Collections;

public class Lvl5_Start : MonoBehaviour {

	public GameObject player;
	public GameObject[] meteorSpawnObjs;
	public GameObject chatHandlerObj;
	private ChatHandler ch;
	private Vector3 startPos;

	private bool done = false;

	void Start () {
		startPos = player.transform.position;
		ch = chatHandlerObj.GetComponent<ChatHandler> ();
	}
	
	void checkChange(){
		if (!done){
			if (!ch.isInChat()){
				foreach (GameObject g in meteorSpawnObjs){
					MeteorSpawn ms = g.GetComponent<MeteorSpawn> ();
					ms.spawnMeteors = true;
				}
				done = true;
			}
		}
	}

	void Update () {
		checkChange ();
	}
}
