using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarrotGardenBehaviour : MonoBehaviour {

	public GameObject carrotPrefab;
	private Vector3[] savedPostitions;
	private Vector3[] savedScales;

	void Start () {
		savedPostitions = new Vector3[transform.childCount];
		savedScales = new Vector3[transform.childCount];
		saveCarrots ();
	}

	private void saveCarrots(){
		int i = 0;
		foreach (Transform child in transform) {
			savedPostitions[i] = child.position;
			savedScales[i] = child.localScale;
			i++;
		}
	}

	public void reset(){
		ExtraFunctions.destroyChildren (transform);

		for (int i=0;i<this.savedPostitions.Length;i++){
			GameObject carrot = GameObject.Instantiate(carrotPrefab) as GameObject;
			carrot.transform.position = savedPostitions[i];
			carrot.transform.localScale = savedScales[i];
			carrot.transform.parent = transform;

		}

	}

	void Update () {
	
	}
}
