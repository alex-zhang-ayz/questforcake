using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BunnyController : MonoBehaviour, Spawner {

	public GameObject player;
	public GameObject carrotGarden;
	public GameObject escapeHole;
	public GameObject respawnObj;
	public GameObject carrotRespawnObj;
	private RespawnableObjects ro, cro;
	private List<GameObject> carrots;
	private bool started;
	private bool finished;
	private bool failed;

	private bool hasUpdatedThings = false;

	void Start () {
		started = false;
		finished = false;
		failed = true;
		ro = respawnObj.GetComponent<RespawnableObjects> ();
		cro = carrotRespawnObj.GetComponent<RespawnableObjects> ();

		carrots = new List<GameObject> ();
	}

	private void updateLocalCarrotList(){
		carrots.Clear ();
		foreach (Transform child in carrotGarden.transform) {
			carrots.Add(child.gameObject);
		}
	}

	public bool getFinish(){
		return finished;
	}
	

	public void resetAll(){
		cro.removeContainerObjects ();
		cro.copyObjects ();
		updateLocalCarrotList ();
		reset ();
	}

	public bool getFailed(){
		return finished && failed;
	}

	private void reset(){


		started = false;
		finished = false;
		failed = true;
		ExtraFunctions.destroyChildren (this.transform);
		ro.copyObjects ();

		int i = 0;
		foreach (Transform child in transform) {
			if (i > 0){
				BunnyBehaviour bb = child.GetComponent<BunnyBehaviour>();
				bb.parentBunny = transform.GetChild(i - 1).gameObject;
				bb.escapeHole = escapeHole;
			}
			i++;
		}
	}

	public GameObject getNewCarrot(GameObject g){
		carrots.Remove (g);
		Destroy (g);
		return getNewCarrot ();

	}

	public GameObject getNewCarrot(){
		if (carrots.Count > 0) {
			int value = Mathf.RoundToInt(Random.Range(0, carrots.Count - 1));
			return carrots [value];
		} else {
			return null;
		}
	}

	public void allFlee(){
		foreach (Transform child in transform) {
			BunnyBehaviour bb = child.GetComponent<BunnyBehaviour>();
			if (bb != null){
				bb.flee = true;
			}
		}
	}

	private void checkFinish(){
		if (transform.childCount == 0) {
			failed = false;
			finished = true;
		}
		bool b = true;
		foreach (Transform child in transform) {
			BunnyBehaviour bb = child.GetComponent<BunnyBehaviour>();
			if (!bb.getFinished()){
				b = false;
			}
		}
		finished = b;
	}

	public void startSpawn(){
		if (!started) {
			int i=0;
			foreach(Transform child in transform){
				child.gameObject.SetActive(true);
				BunnyBehaviour bn = child.GetComponent<BunnyBehaviour>();
				bn.carrot = carrots[i];

				if (i >= carrots.Count ){
					i = 0;
				}else{
					i++;
				}
			}
			started = true;
		}
	}

	void Update () {
		if (!hasUpdatedThings) {
			reset ();
			updateLocalCarrotList ();
			hasUpdatedThings = true;
		}
		if (started && !finished) {
			checkFinish ();
		}
	}
}
