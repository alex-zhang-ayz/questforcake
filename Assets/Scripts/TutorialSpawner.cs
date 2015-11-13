using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TutorialSpawner : MonoBehaviour, Spawner {

	public GameObject respawnableController;
	private RespawnableObjects ro;
	private bool started = false;
	private bool finished = false;
	private bool failed = false;
	private int baseChildCount = -1;
	private GameObject[] minmax;

	void Start () {
		minmax = new GameObject[2];
		if (respawnableController != null) {
			ro = respawnableController.GetComponent<RespawnableObjects> ();
		}
	}

	private void findMinMax(){
		Dictionary<float, GameObject> d = new Dictionary<float, GameObject> ();
		if (transform.childCount > 1){
			int i=0;
			foreach (Transform child in transform) {
				DestructibleBehaviour db = child.GetComponent<DestructibleBehaviour>();
				if (db != null){
					d.Add(db.getCurrentVolume(), child.gameObject);
					i++;
				}
			}
			minmax [0] = d[d.Keys.Min ()];
			minmax [1] = d[d.Keys.Max ()];
		}

	}


	public void startSpawn(){
		if (!started) {
			foreach (Transform child in transform) {
				child.gameObject.SetActive (true);
			}
			if (respawnableController != null){
				baseChildCount = transform.childCount;
				findMinMax();
			}
			started = true;
		}
	}
	public bool getFinish(){
		return finished;
	}
	public bool getFailed(){
		return failed && finished;
	}
	public void resetAll (){
		if (ro != null) {
			ExtraFunctions.destroyChildren(transform);
			ro.copyObjects();
			started = false;
			finished = false;
			failed = false;
		}
	}

	private void checkFinished(){
		if (ro != null) {
			if (started) {
				bool minMaxCheck = false;
				if (minmax [0] == null && minmax [1] == null) {
					minMaxCheck = true;
				}
				if (transform.childCount == baseChildCount - 2 && minMaxCheck) {
					finished = true;
				}

			}
		} else {
			if (started && transform.childCount == 0){
				finished = true;
			}
		}
	}

	private void checkFailed(){
		if (baseChildCount > 0 && started) {
			bool minMaxCheck = false;
			if (minmax[0] == null && minmax[1] == null){
				minMaxCheck = true;
			}
			if (minMaxCheck && transform.childCount < baseChildCount - 2){
				failed = true;
				finished = true;
			}
			if (!minMaxCheck && transform.childCount <= baseChildCount - 2){
				failed = true;
				finished = true;
			}

		}
	}

	void Update () {
		checkFailed ();
		checkFinished ();
	}
}
